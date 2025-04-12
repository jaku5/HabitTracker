using System;
using HabitTrackerLibrary;
using HabitTrackerWebApp.Models;

namespace HabitTrackerWebApp.Services;

public static class HabitTrackerService
{
    static string userDataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HabitTrackerCLI", "habit_data.json");
    private static readonly HabitTracker habitTracker;
    static HabitTrackerService()
    {
        habitTracker = new HabitTracker(userDataFilePath);
    }

    public static List<HabitToTrack> GetAllHabitsAsObjects()
    {
        return habitTracker.HabitsToTrack
            .Select(habit => new HabitToTrack { Name = habit })
            .ToList();
    }

    public static void Initialize()
    {
        if (!habitTracker.LoadUserData())
        {
            habitTracker.AddHabit("Example Habit");
            habitTracker.SaveUserData();
        }
    }

    public static bool IsValidHabitName(HabitToTrack habitToTrack)
    {
        return HabitTracker.IsValidHabitName(habitToTrack.Name);
    }

    public static List<HabitToTrack> GetAll()
    {
        return GetAllHabitsAsObjects();
    }

    public static HabitToTrack? Get(string name)
    {
        return GetAllHabitsAsObjects().FirstOrDefault(h => h.Name == name);
    }

    public static void Add(HabitToTrack habitToTrack)
    {
        habitTracker.AddHabit(habitToTrack.Name!);
    }
}
