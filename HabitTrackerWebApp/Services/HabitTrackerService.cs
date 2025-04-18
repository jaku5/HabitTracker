using System;
using HabitTrackerLibrary;
using HabitTrackerWebApp.Models;

namespace HabitTrackerWebApp.Services;

public static class HabitTrackerService
{
    static string userDataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HabitTrackerCLI", "habit_data.json");
    private static readonly HabitTracker habitTracker;

    static List<HabitToTrack> HabitsToTrack { get; set; }

    static int id = 1;

    static HabitTrackerService()
    {
        habitTracker = new HabitTracker(userDataFilePath);
        HabitsToTrack = GetAllHabitsAsObjects();
    }

    public static List<HabitToTrack> GetAllHabitsAsObjects()
    {
        Initialize();

        return habitTracker.HabitsToTrack
            .Select(habit => new HabitToTrack { Id = id++, Name = habit.Name, CompletionDates = habit.CompletionDates })
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

    public static void ReloadHabits()
    {
        id = 1;
        HabitsToTrack = GetAllHabitsAsObjects();
    }

    public static bool IsValidHabitName(HabitToTrack habitToTrack)
    {
        return HabitTracker.IsValidHabitName(habitToTrack.Name);
    }

    public static List<HabitToTrack>? GetAll()
    {
        return HabitsToTrack;
    }

    public static HabitToTrack? Get(int id)
    {
        return HabitsToTrack?.FirstOrDefault(h => h.Id == id);
    }

    public static void Add(HabitToTrack habitToTrack)
    {
        habitToTrack.Id = id++;
        habitTracker.AddHabit(habitToTrack.Name!);

        ReloadHabits();
    }

    public static void MarkHabitDone(HabitToTrack habitToTrack, DateOnly date)
    {
        habitTracker.MarkHabitDone(habitToTrack.Name, date);

        ReloadHabits();
    }

    public static void Update(HabitToTrack habitToRename, HabitToTrack habitToTrack)
    {
        var index = HabitsToTrack.FindIndex(h => h.Id == habitToTrack.Id);
        if (index == -1)
            return;

        HabitsToTrack[index] = habitToTrack;
        habitTracker.RenameHabit(habitToRename.Name, habitToTrack.Name);

        ReloadHabits();
    }

    public static void Delete(int id)
    {
        var habit = Get(id);
        if (habit is null)
            return;

        habitTracker.RemoveHabit(habit.Name);

        ReloadHabits();
    }
}
