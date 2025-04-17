using System.Text.Json;

namespace HabitTrackerLibrary;

public class HabitTracker
{
    private readonly string _userDataFilePath;
    private List<Habit> _habitsToTrack = new List<Habit>();

    private static DateTime _currentDate = DateTime.Now;
    private DateTime _selectedDate = _currentDate;

    private DayOfWeek _firstDayOfWeek = DayOfWeek.Monday;

    public HabitTracker(string userDataFilePath)
    {
        _userDataFilePath = userDataFilePath;
    }

    public List<Habit> HabitsToTrack
    {
        get => _habitsToTrack;
        private set => _habitsToTrack = value;
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

    public void RemoveHabit(string habitName)
    {
        var habit = HabitsToTrack.FirstOrDefault(h => h.Name == habitName);
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

    public void AddHabit(string habitName)
    {
        //@TODO: Check for duplicate habit names
        HabitsToTrack.Add(new Habit { Name = habitName });
        SaveUserData();
    }

    public void MarkHabitDone(string habitName, DateOnly date)
    {
        var habit = HabitsToTrack.FirstOrDefault(h => h.Name == habitName);

        if (habit != null)
        {
            if (habit.CompletionDates.Contains(date))
                habit.CompletionDates.Remove(date);

            else
                habit.CompletionDates.Add(date);

            SaveUserData();
        }
    }

    public void RenameHabit(string oldHabitName, string newHabitName)
    {
        var habit = HabitsToTrack.FirstOrDefault(h => h.Name == oldHabitName);
        int habitIndex = HabitsToTrack.IndexOf(habit);
        if (habitIndex != -1)
        {
            HabitsToTrack[habitIndex].Name = newHabitName;
        }

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

                if (userData?.HabitsToTrack.Count > 0)
                {
                    _habitsToTrack = userData.HabitsToTrack;
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

    public int CalculateCurrentStreak(string habitName)
    {
        var habit = HabitsToTrack.FirstOrDefault(h => h.Name == habitName);

        return habit.CalculateCurrentStreak();
    }

    public int CalculateRecordStreak(string habitName)
    {
        var habit = HabitsToTrack.FirstOrDefault(h => h.Name == habitName);
        
        return habit.CalculateRecordStreak();
    }
}