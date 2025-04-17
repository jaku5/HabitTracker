using System;

namespace HabitTrackerLibrary;

public class Habit
{
    public string Name { get; set; }
    public List<DateOnly> CompletionDates { get; set; } = new List<DateOnly>();

    //@TODO: Handle 0001-01-01 edge case exception

    public int CalculateCurrentStreak()
    {
        int streak = 0;
        DateOnly currentDate = DateOnly.FromDateTime(HabitTracker.CurrentDate);

        while (CompletionDates.Contains(currentDate))
        {
            streak++;
            currentDate = currentDate.AddDays(-1);
        }

        return streak;
    }

    public int CalculateRecordStreak()
    {
        if (CompletionDates.Count == 0)
            return 0;
            
        var sortedDates = CompletionDates.OrderBy(d => d).ToList();
        int recordStreak = 0, tempStreak = 1;

        for (int i = 1; i < sortedDates.Count; i++)
        {
            if (sortedDates[i] == sortedDates[i - 1].AddDays(1))
            {
                tempStreak++;
            }
            else
            {
                recordStreak = Math.Max(recordStreak, tempStreak);
                tempStreak = 1;
            }
        }

        return Math.Max(recordStreak, tempStreak);
    }

    internal void MarkDone(DateOnly date)
    {
        if (!CompletionDates.Contains(date))
            CompletionDates.Add(date);
        else
            CompletionDates.Remove(date);
    }
}
