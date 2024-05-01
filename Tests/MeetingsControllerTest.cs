public class MeetingsControllerTests
{
    private MeetingsController meetingsController;

    public MeetingsControllerTests()
    {
        meetingsController = new MeetingsController();
    }

    public void TestAddMeeting()
    {
        Meeting meeting = new Meeting(DateTime.Now, DateTime.Now.AddHours(1), DateTime.Now.AddMinutes(30), "Test Add Meeting", "Test Add Location");

        meetingsController.AddMeeting(meeting);

        var addedMeeting = meetingsController.GetMeetingByDateTime(meeting.BeginningTime);

        if (addedMeeting == null)
        {
            Console.WriteLine("TestAddMeeting не прошел: Встреча не была добавлена.");
        }
        else if (addedMeeting.Description != meeting.Description || addedMeeting.Location != meeting.Location || addedMeeting.BeginningTime != meeting.BeginningTime || addedMeeting.EndTime != meeting.EndTime || addedMeeting.ReminderTime != meeting.ReminderTime)
        {
            Console.WriteLine("TestAddMeeting не прошел: Детали встречи не совпадают.");
        }
        else
        {
            Console.WriteLine("TestAddMeeting прошел успешно.");
        }
    }

    public void TestDeleteMeeting()
    {
        Meeting meeting = new Meeting(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3), DateTime.Now.AddHours(1), "Test Delete Meeting", "Test Delete Location");

        meetingsController.AddMeeting(meeting);

        var addedMeeting = meetingsController.GetMeetingByDateTime(meeting.BeginningTime);
        if (addedMeeting == null)
        {
            Console.WriteLine("TestDeleteMeeting не может быть выполнен: Встреча не была добавлена.");
            return;
        }

        meetingsController.DeleteMeeting(meeting);

        var deletedMeeting = meetingsController.GetMeetingByDateTime(meeting.BeginningTime);

        if (deletedMeeting != null)
        {
            Console.WriteLine("TestDeleteMeeting не прошел: Встреча не была удалена.");
        }
        else
        {
            Console.WriteLine("TestDeleteMeeting прошел успешно.");
        }
    }

    public void TestUpdateMeeting()
    {
        Meeting oldMeeting = new Meeting(DateTime.Now.AddHours(4), DateTime.Now.AddHours(5), DateTime.Now.AddHours(3), "Test Update Meeting", "Test Update Location");
        Meeting newMeeting = new Meeting(DateTime.Now.AddHours(4), DateTime.Now.AddHours(5), DateTime.Now.AddHours(3), "Updated Meeting", "Updated Location");

        meetingsController.AddMeeting(oldMeeting);
        meetingsController.UpdateMeeting(oldMeeting, newMeeting);

        var updatedMeeting = meetingsController.GetMeetingByDateTime(newMeeting.BeginningTime);

        if (updatedMeeting == null)
        {
            Console.WriteLine("TestUpdateMeeting не прошел: Встреча не была обновлена.");
        }
        else if (updatedMeeting.Description != newMeeting.Description || updatedMeeting.Location != newMeeting.Location)
        {
            Console.WriteLine("TestUpdateMeeting не прошел: Детали встречи не совпадают.");
        }
        else
        {
            Console.WriteLine("TestUpdateMeeting прошел успешно.");
        }
    }

    public void RunTests()
    {
        TestAddMeeting();
        TestDeleteMeeting();
        TestUpdateMeeting();
    }

    // опустошить meetings.json и запустить тесты

    // в Main
    // MeetingsControllerTests tests = new MeetingsControllerTests();
    // tests.RunTests();
}