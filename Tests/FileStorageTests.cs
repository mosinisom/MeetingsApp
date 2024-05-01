public class FileStorageTests
{
    private FileStorage fileStorage;
    private string testFilePath = "testFile.json";

    public FileStorageTests()
    {
        fileStorage = new FileStorage(testFilePath);
    }

    public void TestSaveAndLoadMeetings()
    {
        List<Meeting> meetings = new List<Meeting>
        {
            new Meeting(DateTime.Now, DateTime.Now.AddHours(1), DateTime.Now.AddMinutes(30), "Test Meeting", "Test Location")
        };

        fileStorage.SaveMeetings(meetings);
        var loadedMeetings = fileStorage.LoadMeetings();

        if (loadedMeetings.Count != meetings.Count || loadedMeetings[0].Description != meetings[0].Description)
        {
            Console.WriteLine("TestSaveAndLoadMeetings не прошел: сохраненные и загруженные встречи не совпадают.");
        }
        else
        {
            Console.WriteLine("TestSaveAndLoadMeetings прошел успешно.");
        }

        File.Delete(testFilePath);
    }

    public void TestGetMeetingsByDay()
    {
        DateTime testDate = DateTime.Now.Date;
        List<Meeting> meetings = new List<Meeting>
        {
            new Meeting(testDate, testDate.AddHours(1), testDate.AddMinutes(30), "Test Meeting", "Test Location"),
            new Meeting(testDate.AddDays(1), testDate.AddDays(1).AddHours(1), testDate.AddDays(1).AddMinutes(30), "Test Meeting 2", "Test Location 2")
        };
        fileStorage.SaveMeetings(meetings);

        var meetingsByDay = fileStorage.GetMeetingsByDay(testDate);

        if (meetingsByDay.Count != 1 || meetingsByDay[0].Description != meetings[0].Description)
        {
            Console.WriteLine("TestGetMeetingsByDay не прошел: встречи за день не совпадают.");
        }
        else
        {
            Console.WriteLine("TestGetMeetingsByDay прошел успешно.");
        }

        File.Delete(testFilePath);
    }

    public void RunTests()
    {
        TestSaveAndLoadMeetings();
        TestGetMeetingsByDay();
    }

    // FileStorageTests fileStorageTests = new FileStorageTests();
    // fileStorageTests.RunTests();
}