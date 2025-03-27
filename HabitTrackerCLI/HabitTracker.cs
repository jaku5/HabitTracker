using System.Text.Json;

namespace HabitTrackerCLI;

public class HabitTracker
{
    private List<string> _habitsToTrack = new List<string>();
    private List<string> _habitsCompleted = new List<string>();

    private static DateTime _currentDate = DateTime.Now;
    private DateTime _selectedDate = _currentDate;

    private DayOfWeek _firstDayOfWeek = DayOfWeek.Monday;

    public List<string> HabitsToTrack
    {
        get => _habitsToTrack;
        set => _habitsToTrack = value;
    }

    public List<string> HabitsCompleted
    {
        get => _habitsCompleted;
        set => _habitsCompleted = value;
    }

    public DateTime CurrentDate
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

    public void ModifyHabitList(string habit)
    {
        if (_habitsToTrack.Contains(habit))
        {
            for (int i = 0; i < _habitsCompleted.Count; i++)
            {
                while (_habitsCompleted.Count > 0 && _habitsCompleted[i].ToString().Substring(0, habit.Length).Equals(habit))
                {
                    MarkHabitDone(_habitsCompleted[i]);

                    if (i > _habitsCompleted.Count - 1) break;
                }
            }

            if (_habitsToTrack.Count > 1)
            {
                _habitsToTrack.Remove(habit);
            }

            else
            {
                _habitsToTrack.Remove(habit);
                SaveUserData();
                LoadUserData();
            }
        }

        else
        {
            _habitsToTrack.Add(habit);
        }

        SaveUserData();
    }

    public void MarkHabitDone(string habitEntryID)
    {
        if (_habitsCompleted.Contains(habitEntryID))
            _habitsCompleted.Remove(habitEntryID);

        else
            _habitsCompleted.Add(habitEntryID);

        SaveUserData();
    }

    public void RenameHabit(string oldHabitName, string newHabitName)
    {
        int habitIndex = HabitsToTrack.IndexOf(oldHabitName);
        if (habitIndex != -1)
        {
            HabitsToTrack[habitIndex] = newHabitName;
        }

        for (int i = 0; i < HabitsCompleted.Count; i++)
        {
            if (HabitsCompleted[i].StartsWith(oldHabitName))
            {
                string datePart = HabitsCompleted[i].Substring(oldHabitName.Length);
                HabitsCompleted[i] = newHabitName + datePart;
            }
        }

        SaveUserData();
    }

    public bool LoadUserData()
    {
        bool dataLoaded = false;

        if (File.Exists("./habit_data.json"))
        {
            string jsonData = File.ReadAllText("./habit_data.json");
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

    public void SaveUserData()
    {
        var userData = new UserData
        {
            HabitsToTrack = _habitsToTrack,
            HabitsCompleted = _habitsCompleted,
            FirstDayOfWeek = _firstDayOfWeek
        };

        string jsonData = JsonSerializer.Serialize(userData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("./habit_data.json", jsonData);
    }

    public int CalculateCurrentStreak(string habit)
    {
        int currentStreak = 0;
        DateOnly currentHabitDate = DateOnly.FromDateTime(CurrentDate);
        string currentHabitID = $"{habit}{currentHabitDate.ToString("yyyy-MM-dd")}";

        while (HabitsCompleted.Contains(currentHabitID))
        {
            currentHabitDate = currentHabitDate.AddDays(-1);
            currentHabitID = habit + currentHabitDate.ToString("yyyy-MM-dd");
            currentStreak++;
        }

        return currentStreak;
    }

    public int CalculateRecordStreak(string habit)
    {
        int recordStreak = 0;

        foreach (string habitCompletedID in HabitsCompleted)
        {
            DateOnly currentHabitDate;

            int tempRecordStreak = 0;

            if (habitCompletedID.ToString().Contains(habit))
            {
                DateOnly.TryParse(habitCompletedID.Substring(habit.Length), out currentHabitDate);
                string currentHabitID = $"{habit}{currentHabitDate.ToString("yyyy-MM-dd")}";

                while (HabitsCompleted.Contains(currentHabitID))
                {
                    currentHabitDate = currentHabitDate.AddDays(-1);
                    currentHabitID = habit + currentHabitDate.ToString("yyyy-MM-dd");
                    tempRecordStreak++;
                }

                if (tempRecordStreak > recordStreak)
                    recordStreak = tempRecordStreak;
            }
        }

        return recordStreak;
    }
}
