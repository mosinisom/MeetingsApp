public class MeetingsController
{
    FileStorage fileStorage = new FileStorage("meetings.json");
    List<Meeting> meetings = new List<Meeting>();

    public MeetingsController()
    {
        GetMeetingsFromStorage();
        MeetingReminder = null;
    }

    public void AddMeeting(Meeting meeting)
    {
        if (CheckCollision(meeting))
            return;

        meetings.Add(meeting);
        fileStorage.SaveMeetings(meetings);
    }

    public event Action<string>? MeetingReminder;

    public List<Meeting> GetMeetingsReminding()
    {
        DateTime now = DateTime.Now;
        DateTime nextMinute = now.AddMinutes(1);
        return meetings.Where(m => m.ReminderTime >= now && m.ReminderTime < nextMinute).ToList();
    }

    public void DeleteMeeting(Meeting meeting)
    {
        meetings.Remove(meeting);
        fileStorage.SaveMeetings(meetings);
    }

    public void UpdateMeeting(Meeting oldMeeting, Meeting newMeeting)
    {
        var newMeetings = new List<Meeting>(meetings);
        newMeetings.Remove(oldMeeting);

        if (CheckCollision(newMeeting, newMeetings))
            return;

        meetings.Remove(oldMeeting);
        meetings.Add(newMeeting);
        fileStorage.SaveMeetings(meetings);
    }

    public List<Meeting> GetSchedule(DateTime day)
    {
        return fileStorage.GetMeetingsByDay(day);
    }

    public void ExportSchedule(DateTime day)
    {
        fileStorage.ExportMeetings(day);
    }

    public void GetMeetingsFromStorage()
    {
        meetings = fileStorage.LoadMeetings();
        if (meetings == null)
        {
            meetings = new List<Meeting>();
            meetings.Add(new Meeting(DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, "", ""));
        }
    }

    public Meeting GetMeetingByDateTime(DateTime dateTime)
    {
        Meeting? meeting = meetings.FirstOrDefault(m => m.BeginningTime == dateTime);
        return meeting;
    }

    public List<Meeting> GetMeetings()
    {
        return meetings;
    }

    public List<Meeting> GetMeetingsByDay(DateTime day)
    {
        return meetings.Where(m => m.BeginningTime.Date == day.Date).ToList();
    }

    public List<Meeting> GetFutureOrPastMeetings(bool isFuture)
    {
        DateTime time = DateTime.Now;
        return isFuture ? meetings.Where(m => m.BeginningTime > time).ToList() : meetings.Where(m => m.BeginningTime < time).ToList();
    }

    public bool CheckCollision(Meeting meeting, List<Meeting>? meetingsForCheck = null)
    {
        if (meetingsForCheck == null)
            meetingsForCheck = meetings;

        return meetingsForCheck.Any(m => m != meeting &&
            ((m.BeginningTime <= meeting.BeginningTime && m.EndTime > meeting.BeginningTime) ||
            (m.BeginningTime < meeting.EndTime && m.EndTime >= meeting.EndTime) ||
            (m.BeginningTime > meeting.BeginningTime && m.EndTime < meeting.EndTime)));
    }
}