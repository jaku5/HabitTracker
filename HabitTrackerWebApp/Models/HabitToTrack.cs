using System;

namespace HabitTrackerWebApp.Models;

public class HabitToTrack
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<DateOnly>? CompletionDates { get; set; }
    public int CurrentStreak { get; set; }
    public int RecordStreak { get; set; }
}
