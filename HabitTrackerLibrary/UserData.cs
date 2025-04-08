namespace HabitTrackerLibrary;

public class UserData
{
    public List<string> HabitsToTrack { get; set; } = [];
    public HashSet<string> HabitsCompleted { get; set; } = [];
    public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Monday;
}