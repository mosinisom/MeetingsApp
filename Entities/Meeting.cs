public class Meeting
{
    public DateTime BeginningTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime ReminderTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Meeting() { }

    public Meeting(DateTime begginingTime, DateTime endTime, DateTime reminderTime, string location, string description)
    {
        BeginningTime = begginingTime;
        EndTime = endTime;
        ReminderTime = reminderTime;
        Location = location;
        Description = description;
    }

    public override string ToString()
    {
        return $"\"{Description}\" с {BeginningTime:yyyy-MM-dd HH:mm} до {EndTime:yyyy-MM-dd HH:mm} в {Location}";
    }
}