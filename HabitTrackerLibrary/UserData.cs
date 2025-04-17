namespace HabitTrackerLibrary;

public class UserData
{
    public List<Habit> HabitsToTrack { get; set; } = [];
    public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Monday;
}