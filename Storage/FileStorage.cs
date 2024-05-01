using System.Text.Json;

public class FileStorage
{
    public string FilePath { get; init; }

    public FileStorage(string filePath)
    {
        FilePath = filePath;
    }
    public void SaveMeetings(List<Meeting> meetings)
    {
        string json = JsonSerializer.Serialize(meetings);
        File.WriteAllText(FilePath, json);
    }

    public List<Meeting> LoadMeetings()
    {
        if (!File.Exists(FilePath))
            return new List<Meeting>();
        
        string json = File.ReadAllText(FilePath);
        if (string.IsNullOrEmpty(json))
            return new List<Meeting>();
        return JsonSerializer.Deserialize<List<Meeting>>(json);
    }

    public void ExportMeetings(DateTime day)
    {
        List<Meeting> meetings = LoadMeetings();
        List<Meeting> dayMeetings = meetings.Where(m => m.BeginningTime.Date == day.Date).ToList();
        string text = string.Join(Environment.NewLine, dayMeetings);
        string filePath = $"meetings_{day:yyyy-MM-dd}.txt";
        File.WriteAllText(filePath, text);
    }

    public List<Meeting> GetMeetingsByDay(DateTime day)
    {
        List<Meeting> meetings = LoadMeetings();
        return meetings.Where(m => m.BeginningTime.Date == day.Date).ToList();
    }
}