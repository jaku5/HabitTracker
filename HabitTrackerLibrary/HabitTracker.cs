using System.Text.Json;

namespace HabitTrackerLibrary;

public class HabitTracker
{
    private readonly string _userDataFilePath;
    private List<string> _habitsToTrack = new List<string>();
    private HashSet<string> _habitsCompleted = new HashSet<string>();

    private static DateTime _currentDate = DateTime.Now;
    private DateTime _selectedDate = _currentDate;

    private DayOfWeek _firstDayOfWeek = DayOfWeek.Monday;

    public HabitTracker(string userDataFilePath)
    {
        _userDataFilePath = userDataFilePath;
    }

    public List<string> HabitsToTrack
    {
        get => _habitsToTrack;
        private set => _habitsToTrack = value;
    }

    public HashSet<string> HabitsCompleted
    {
        get => _habitsCompleted;
        private set => _habitsCompleted = value;
    }

    public static DateTime CurrentDate
    {
        get => _currentDate;
    }

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set => _selectedDate = value;
    }

    public DayOfWeek FirstDayOfWeek
    {
        get => _firstDayOfWeek;
        set => _firstDayOfWeek = value;
    }

    public void RemoveHabit(string habit)
    {
        var itemsToRemove = new HashSet<string>();

        foreach (var habitCompleted in HabitsCompleted)
        {
            if (habitCompleted.StartsWith(habit))
            {
                itemsToRemove.Add(habitCompleted);
            }
        }

        foreach (var item in itemsToRemove)
        {
            MarkHabitDone(item);
        }

        HabitsToTrack.Remove(habit);

        if (HabitsToTrack.Count == 0)
        {
            SaveUserData();
            LoadUserData();
        }

        else
        {
            SaveUserData();
        }
    }

    public void AddHabit(string habit)
    {
        HabitsToTrack.Add(habit);
        SaveUserData();
    }

    public void MarkHabitDone(string habitEntryId)
    {
        if (HabitsCompleted.Contains(habitEntryId))
            HabitsCompleted.Remove(habitEntryId);

        else
            HabitsCompleted.Add(habitEntryId);

        SaveUserData();
    }

    public void RenameHabit(string oldHabitName, string newHabitName)
    {
        int habitIndex = HabitsToTrack.IndexOf(oldHabitName);
        if (habitIndex != -1)
        {
            HabitsToTrack[habitIndex] = newHabitName;
        }

        var updatedHabits = new HashSet<string>();

        foreach (var habitCompleted in HabitsCompleted)
        {
            if (habitCompleted.StartsWith(oldHabitName))
            {
                string datePart = habitCompleted.Substring(oldHabitName.Length);
                updatedHabits.Add(newHabitName + datePart);
            }

            else
            {
                updatedHabits.Add(habitCompleted);
            }
        }

        HabitsCompleted = updatedHabits;

        SaveUserData();
    }

    public bool LoadUserData()
    {
        bool dataLoaded = false;
        try
        {
            if (File.Exists(_userDataFilePath))
            {
                string jsonData = File.ReadAllText(_userDataFilePath);
                var userData = JsonSerializer.Deserialize<UserData>(jsonData);

                if (userData.HabitsToTrack.Count > 0)
                {
                    _habitsToTrack = userData.HabitsToTrack;
                    _habitsCompleted = userData.HabitsCompleted;
                    _firstDayOfWeek = userData.FirstDayOfWeek;

                    dataLoaded = true;

                    return dataLoaded;
                }

                else
                {
                    return dataLoaded;
                }
            }

            else
            {
                return dataLoaded;
            }
        }

        catch (JsonException e)
        {
            throw new InvalidDataException("Failed to load user data due to a JSON error.", e);
        }

        catch (IOException e)
        {
            throw new IOException("Failed to load user data due to a I/O error.", e);
        }

        catch (UnauthorizedAccessException e)
        {
            throw new UnauthorizedAccessException("Failed to load user data due to a permission error.", e);
        }
    }

    public void SaveUserData()
    {
        try
        {
            var userData = new UserData
            {
                HabitsToTrack = _habitsToTrack,
                HabitsCompleted = _habitsCompleted,
                FirstDayOfWeek = _firstDayOfWeek
            };

            string jsonData = JsonSerializer.Serialize(userData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_userDataFilePath, jsonData);
        }

        catch (JsonException e)
        {
            throw new InvalidDataException("Failed to save user data due to a JSON error.", e);
        }

        catch (IOException e)
        {
            throw new IOException("Failed to save user data due to a I/O error.", e);
        }

        catch (UnauthorizedAccessException e)
        {
            throw new UnauthorizedAccessException("Failed to save user data due to a permission error.", e);
        }
    }

    public static bool IsValidHabitName(string? habitName)
    {
        return !string.IsNullOrWhiteSpace(habitName) && !habitName.Contains(',') && habitName != null;
    }
    
    public int CalculateCurrentStreak(string habit)
    {
        int currentStreak = 0;
        DateOnly currentHabitDate = DateOnly.FromDateTime(CurrentDate);
        string currentHabitId = $"{habit}{currentHabitDate.ToString("yyyy-MM-dd")}";

        while (HabitsCompleted.Contains(currentHabitId))
        {
            currentHabitDate = currentHabitDate.AddDays(-1);
            currentHabitId = habit + currentHabitDate.ToString("yyyy-MM-dd");
            currentStreak++;
        }

        return currentStreak;
    }

    public int CalculateRecordStreak(string habit)
    {
        int recordStreak = 0;

        foreach (string habitCompletedId in HabitsCompleted)
        {
            DateOnly currentHabitDate;

            int tempRecordStreak = 0;

            if (habitCompletedId.ToString().Contains(habit))
            {
                DateOnly.TryParse(habitCompletedId.Substring(habit.Length), out currentHabitDate);
                string currentHabitId = $"{habit}{currentHabitDate.ToString("yyyy-MM-dd")}";

                while (HabitsCompleted.Contains(currentHabitId))
                {
                    currentHabitDate = currentHabitDate.AddDays(-1);
                    currentHabitId = habit + currentHabitDate.ToString("yyyy-MM-dd");
                    tempRecordStreak++;
                }

                if (tempRecordStreak > recordStreak)
                    recordStreak = tempRecordStreak;
            }
        }

        return recordStreak;
    }
}