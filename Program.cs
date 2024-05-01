using System;
using System.Threading;

class Program
{
    private static Timer? timer;
    static void Main(string[] args)
    {
        int WindowHeight = Console.WindowHeight;
        MeetingsController meetingsController = new MeetingsController();
        UserInterface ui = new UserInterface(meetingsController);

        timer = new Timer(Notify, null, 0, 60000); 


        while (true)
        {
            ui.ShowMenu();
        }

        void Notify(object state)
        {
            var meetings = ui.GetMeetingsReminding();
            int offsetY = 1;
            foreach (var meeting in meetings)
            {
                string message = $"Встреча '{meeting.Description}' с {meeting.BeginningTime:yyyy-MM-dd HH:mm} до {meeting.EndTime:yyyy-MM-dd HH:mm} в {meeting.Location}";
                Util.Print(1, WindowHeight - offsetY++, message, ConsoleColor.Red, ConsoleColor.Yellow);
            }
        }
    }

}