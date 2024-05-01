public class UserInterface
{
    private MeetingsController meetingsController;
    private int WindowHeight = Console.WindowHeight;
    private int WindowWidth = Console.WindowWidth;
    private int MenuOffsetX = 2;
    private int MenuOffsetY = 2;
    private string[] MenuItemsShowSchedule = { "Все будущие встречи", "Выбрать конкретный день", "Все прошлые встречи", "Назад" };
    private string[] MenuItemsExportSchedule = { "Экспорт на сегодня", "Экспорт на выбранный день", "Назад" };

    public UserInterface(MeetingsController meetingsController)
    {
        this.meetingsController = meetingsController;
        this.meetingsController.MeetingReminder += ShowReminder;
    }

    private void ShowReminder(string message)
    {
        Util.Print(1, WindowHeight - 1, message, ConsoleColor.White, ConsoleColor.Yellow);
    }

    public List<Meeting> GetMeetingsReminding()
    {
        return meetingsController.GetMeetingsReminding();
    }

    public void ShowMenu()
    {
        Console.Clear();
        string[] menuItems =
        {
        "Добавить встречу",
        "Удалить встречу",
        "Изменить встречу",
        "Показать расписание",
        "Экспортировать встречи",
        "Выход"
        };

        int selectedMenuItem = Util.Menu(0, 0, menuItems);

        switch (selectedMenuItem)
        {
            case 0:
                AddMeeting(string.Empty, string.Empty);
                break;
            case 1:
                DeleteMeeting();
                break;
            case 2:
                EditMeeting();
                break;
            case 3:
                ShowSchedule();
                break;
            case 4:
                ShowExportMeetings();
                break;
            case 5:
                Environment.Exit(0);
                break;
        }
    }

    public void AddMeeting(string title, string location)
    {
        Console.Clear();
        Console.WriteLine("Добавление новой встречи");

        if (string.IsNullOrEmpty(title))
            title = Util.Input("Введите название встречи: ");

        if (string.IsNullOrEmpty(location))
            location = Util.Input("Введите место встречи: ");

        DateTime startTime = InputStartTime();

        DateTime endTime = InputEndTime(startTime);

        DateTime reminderTime = InputReminderTime(startTime);

        var meeting = new Meeting(startTime, endTime, reminderTime, location, title);

        CheckCollision(title, location, meeting);

        meetingsController.AddMeeting(meeting);
    }

    private void CheckCollision(string title, string location, Meeting meeting)
    {
        if (meetingsController.CheckCollision(meeting))
        {
            Console.WriteLine("Эта встреча пересекается с уже существующей встречей. Пожалуйста, введите другое время.");
            Console.ReadKey();
            AddMeeting(title, location);
            return;
        }
    }

    public bool CheckCollision(Meeting newMeeting, Meeting currentMeeting)
    {
        List<Meeting> meetings = meetingsController.GetMeetings();
        meetings.Remove(currentMeeting);
        if (meetingsController.CheckCollision(newMeeting, meetings))
        {
            Console.Clear();
            Console.WriteLine("Эта встреча пересекается с уже существующей встречей. Пожалуйста, введите другое время.");
            Console.ReadKey();
            return true;
        }
        return false;
    }

    private static DateTime InputReminderTime(DateTime startTime)
    {
        DateTime reminderTime;
        while (true)
        {
            reminderTime = Util.InputDateTime("Введите время напоминания о встрече (формат: ГГГГ-ММ-ДД ЧЧ:ММ): ");
            if (reminderTime > startTime)
                Console.WriteLine("Время напоминания не может быть позже времени начала встречи. Пожалуйста, введите снова.");
            else
                break;
        }

        return reminderTime;
    }

    private static DateTime InputEndTime(DateTime startTime)
    {
        DateTime endTime;
        while (true)
        {
            endTime = Util.InputDateTime("Введите время окончания встречи (формат: ГГГГ-ММ-ДД ЧЧ:ММ): ");
            if (endTime < startTime)
                Console.WriteLine("Время окончания встречи не может быть раньше времени начала. Пожалуйста, введите снова.");
            else
                break;
        }

        return endTime;
    }

    private static DateTime InputStartTime()
    {
        DateTime startTime;
        while (true)
        {
            startTime = Util.InputDateTime("Введите время начала встречи (формат: ГГГГ-ММ-ДД ЧЧ:ММ): ");
            if (startTime < DateTime.Now)
                Console.WriteLine("Время начала встречи не может быть раньше текущего времени. Пожалуйста, введите снова.");
            else
                break;
        }

        return startTime;
    }

    public void DeleteMeeting()
    {
        Console.Clear();
        Console.WriteLine("Удаление встречи");

        DateTime dateTime = Util.InputDateTime("Введите время начала встречи (формат: ГГГГ-ММ-ДД ЧЧ:ММ): ");
        Meeting? meeting = meetingsController.GetMeetingByDateTime(dateTime);
        if (meeting == null || meeting.BeginningTime == DateTime.MinValue)
        {
            Console.WriteLine("Встреча не найдена.");
            Console.ReadKey();
            return;
        }

        meetingsController.DeleteMeeting(meeting);
    }

    public void ShowSchedule()
    {
        Console.Clear();
        Console.WriteLine("Показ расписания");

        int selectedMenuItem = Util.Menu(MenuOffsetX, MenuOffsetY, MenuItemsShowSchedule);
        switch (selectedMenuItem)
        {
            case 0:
                Console.Clear();
                List<Meeting> futureMeetings = meetingsController.GetFutureOrPastMeetings(true);
                futureMeetings = futureMeetings.OrderBy(m => m.BeginningTime).ToList();
                PrintMeetings(futureMeetings);
                Console.ReadKey();
                break;
            case 1:
                Console.Clear();
                DateTime day = Util.InputDateTime("Введите дату (формат: ГГГГ-ММ-ДД):");
                List<Meeting> dayMeetings = meetingsController.GetSchedule(day);
                dayMeetings = dayMeetings.OrderBy(m => m.BeginningTime).ToList();
                PrintMeetings(dayMeetings);
                Console.ReadKey();
                break;
            case 2:
                Console.Clear();
                List<Meeting> pastMeetings = meetingsController.GetFutureOrPastMeetings(false);
                pastMeetings = pastMeetings.OrderBy(m => m.BeginningTime).ToList();
                PrintMeetings(pastMeetings);
                Console.ReadKey();
                break;
            case 3:
                return;

        }
    }

    public void ShowExportMeetings()
    {
        Console.Clear();
        Console.WriteLine("Экспорт расписания");

        int selectedMenuItem = Util.Menu(MenuOffsetX + 2, MenuOffsetY + 2, MenuItemsExportSchedule);
        switch (selectedMenuItem)
        {
            case 0:
                meetingsController.ExportSchedule(DateTime.Now);
                Console.Clear();
                Console.WriteLine("Расписание на сегодня экспортировано.");
                Console.ReadKey();
                break;
            case 1:
                DateTime day = Util.InputDateTime("Введите дату (формат: ГГГГ-ММ-ДД):");
                meetingsController.ExportSchedule(day);
                Console.Clear();
                Console.WriteLine($"Расписание на {day:yyyy-MM-dd} экспортировано.");
                Console.ReadKey();
                break;
            case 2:
                return;

        }
    }

    public void EditMeeting()
    {
        Console.Clear();
        Console.WriteLine("Изменение встречи");

        DateTime dateTime = Util.InputDateTime("Введите время начала встречи (формат: ГГГГ-ММ-ДД ЧЧ:ММ): ");
        Meeting? oldMeeting = meetingsController.GetMeetingByDateTime(dateTime);
        if (oldMeeting == null || oldMeeting.BeginningTime == DateTime.MinValue)
        {
            Console.WriteLine("Встреча не найдена.");
            Console.ReadKey();
            return;
        }

        Meeting meeting = new Meeting(oldMeeting.BeginningTime, oldMeeting.EndTime, oldMeeting.ReminderTime, oldMeeting.Location, oldMeeting.Description);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Что вы хотите изменить?");
            string[] items = { "Название встречи", "Место встречи", "Время начала встречи", "Время окончания встречи", "Время напоминания о встрече", "Завершить изменение" };

            int option = Util.Menu(MenuOffsetX, MenuOffsetY + 1, items);

            switch (option)
            {
                case 0:
                    Console.Clear();
                    meeting.Description = Util.Input("Введите новое название (описание) встречи: ");
                    break;
                case 1:
                    Console.Clear();
                    meeting.Location = Util.Input("Введите новое место встречи: ");
                    break;
                case 2:
                    Console.Clear();
                    meeting.BeginningTime = InputStartTime();
                    break;
                case 3:
                    Console.Clear();
                    meeting.EndTime = InputEndTime(meeting.BeginningTime);
                    break;
                case 4:
                    Console.Clear();
                    meeting.ReminderTime = InputReminderTime(meeting.BeginningTime);
                    break;
                case 5:
                    if (CheckCollision(meeting, oldMeeting))
                        continue;
                    meetingsController.UpdateMeeting(oldMeeting, meeting);
                    return;
            }
        }

    }

    private static void PrintMeetings(List<Meeting> Meetings)
    {
        foreach (var meeting in Meetings)
            Console.WriteLine(meeting.ToString());
    }
}