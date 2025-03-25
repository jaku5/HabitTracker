namespace HabitTrackerCLI;

public class UserData
{
    public List<string> HabitsToTrack { get; set; } = [];
    public List<string> HabitsCompleted { get; set; } = [];
    public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Monday;
}