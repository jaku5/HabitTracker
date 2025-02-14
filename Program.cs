List<string> habitsToTrack = ["Meditate", "Read", "Walk", "Code"];
DayOfWeek[] daysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };
List<string> habitsCompleted = new List<string>();
var currentDate = DateTime.Now;
var selectedDate = currentDate;
DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

// User input helper vriables
string? userInput;
bool exit = false;

do
{
    // Load user data from the txt file
    ModifyHabitList("Write");
    LoadUserData();
    SetFirstDayOfWeek(firstDayOfWeek);
    ShowWeekGrid();
    // Present menu options
    ShowCustomDate(2024, 11, 1);
    ModifyHabitList("Write");
    ModifyHabitList("Read");
    MarkHabitDone("Walk2/12/2025");
    MarkHabitDone("Walk2/11/2025");
    MarkHabitDone("Walk2/10/2025");
    MarkHabitDone("Read2/12/2025", false);
    MarkHabitDone("Read2/11/2025");
    MarkHabitDone("Read2/10/2025");
    MarkHabitDone("Read2/9/2025");
    MarkHabitDone("Read2/8/2025");
    MarkHabitDone("Meditate2/12/2025");
    MarkHabitDone("Read2/10/2025");
    MarkHabitDone("Read2/11/2025");
    MarkHabitDone("Meditate2/14/2025");
    MarkHabitDone("Meditate2/12/2025");
    ModifyHabitList("Read");

    userInput = Console.ReadLine();
    if (userInput != null && userInput.ToLower().Contains("exit"))
    {
        exit = true;
    }
}
while (exit == false);

void LoadUserData()
{
    //@ TODO Handle empty data file
    string habits = "";
    string habitsCompletedID = "";
    StreamReader sr = new StreamReader("./habit_data.txt");

    habits = sr.ReadLine();
    habitsCompletedID = sr.ReadLine();
    habitsToTrack = habits.Split(',').ToList();
    if (habitsCompletedID != "" && habitsCompletedID != null)
        habitsCompleted = habitsCompletedID.Split(',').ToList();
    sr.Close(); 
}

void SaveUserData()
{
    string userData = "";
    string habitList = "";
    string habitCompletedList = "";
    StreamWriter sw = new StreamWriter("./habit_data.txt");

    // @TODO Handle empty habit array
    if (habitsToTrack.Count > 0)
    {
        foreach (string habit in habitsToTrack)
        {
            habitList += $"{habit}, ";
        }

        if (habitsCompleted.Count > 0)
        {
            foreach (string habitCompletedID in habitsCompleted)
            {
                habitCompletedList += $"{habitCompletedID}, ";
            }

            userData = $"{habitList.Remove(habitList.Length - 2)}\n{habitCompletedList.Remove(habitCompletedList.Length - 2)}";
        }

        else
        {
            userData = $"{habitList.Remove(habitList.Length - 2)}";
        }

        sw.WriteLine(userData);
        sw.Close();
    }
}

void ModifyHabitList(string habit)
{
    if (habitsToTrack.Contains(habit))
    {
        for (int i = 0; i < habitsCompleted.Count; i++)
        {
            while (habitsCompleted[i].ToString().Contains(habit))
            {
                MarkHabitDone(habitsCompleted[i], false);
            }
        }

        habitsToTrack.Remove(habit);
    }

    else
    {
        habitsToTrack.Add(habit);
    }

    SaveUserData();
    ShowWeekGrid();
}

void ShowWeekGrid()
{
    Console.Clear();
    Console.WriteLine($"\n\t\tWelcome to Habit Tracker. Current date is {currentDate.DayOfWeek} {DateOnly.FromDateTime(currentDate)}.\n\t\tSelected date is {selectedDate.DayOfWeek} {DateOnly.FromDateTime(selectedDate)}.\n");

    ShowGridHeader();
    ShowGridBody();
}

void ShowGridHeader()
{
    string weekGridHeader = "";
    string weekGridHeaderDates = "";

    for (int i = 0; i < daysOfWeek.Length; i++)
    {
        string currentWeekDay = $"{daysOfWeek[i].ToString()} ";
        string currentWeekDayDate = $"{CalculateGridDates(habitsToTrack[0], i).ToString("dd MMM")} ";

        if (i != 0)
            currentWeekDayDate = $"{CalculateGridDates(habitsToTrack[0], i).ToString("dd MMM").PadLeft(daysOfWeek[i - 1].ToString().Length)} ";

        weekGridHeader += currentWeekDay;
        weekGridHeaderDates += currentWeekDayDate;
    }
    Console.WriteLine($"\t\t{weekGridHeader} Current Record");
    Console.WriteLine($"\t\t{weekGridHeaderDates}\n");
}

void ShowGridBody()
{
    foreach (string habit in habitsToTrack)
    {
        int currentStreak = CalculateCurrentStreak(habit);
        int recordStreak = CalculateRecordStreak(habit);

        string habitCheckRow = "";
        string streaksRow = "";
        string habitEntryID = "";

        for (int i = 0; i < daysOfWeek.Length; i++)
        {
            string currentWeekDay = "";
            DateOnly habitDate = CalculateGridDates(habit, i);
            string checkIcon = "- [x] ";
            string uncheckIcon = "- [ ] ";
            string icon = uncheckIcon;
            bool habitDone = false;
            habitEntryID = $"{habit}{habitDate}";

            if (habitsCompleted.Contains(habitEntryID))
            {
                habitDone = true;
                icon = checkIcon;
            }

            if (i == 0)
            {
                currentWeekDay = icon;
                habitCheckRow += currentWeekDay; // + habitDate;
            }

            else if (i == daysOfWeek.Length - 1)
            {
                currentWeekDay = $"{icon}".PadLeft(daysOfWeek[i - 1].ToString().Length + 1);
                habitCheckRow += currentWeekDay;

                streaksRow = $"{currentStreak}".ToString().PadLeft(daysOfWeek[i].ToString().Length - 3) + $"{recordStreak}".ToString().PadLeft(8);
            }

            else
            {
                currentWeekDay = $"{icon}".PadLeft(daysOfWeek[i - 1].ToString().Length + 1);
                habitCheckRow += currentWeekDay;
            }
        }
        Console.Write($"{habit}");
        // @TODO: Handle long habit names.
        Console.WriteLine($"{habitCheckRow}".PadLeft(72 - habit.Length) + $"{streaksRow}\n");
    }
}

DateOnly CalculateGridDates(string habit, int weekDay)
{

    DateOnly habitGridEntryDate = new DateOnly();
    int dateDaysDifference = 0;


    if (daysOfWeek[weekDay] == selectedDate.DayOfWeek)
    {
        habitGridEntryDate = DateOnly.FromDateTime(selectedDate);
    }

    else
    {
        dateDaysDifference = weekDay - Array.IndexOf(daysOfWeek, selectedDate.DayOfWeek);
        DateTime habitEntryCalculatedDate = new DateTime();

        habitEntryCalculatedDate = selectedDate.AddDays(dateDaysDifference);
        habitGridEntryDate = DateOnly.FromDateTime(habitEntryCalculatedDate);
    }

    return habitGridEntryDate;
}

void SetFirstDayOfWeek(DayOfWeek customFirstDay)
{
    int dayOfWeekOffset = 0;

    if (customFirstDay != DayOfWeek.Sunday)
    {
        dayOfWeekOffset = 7 - (int)customFirstDay;

        daysOfWeek[0] = customFirstDay;
        for (int i = 1; i < dayOfWeekOffset; i++)
        {
            daysOfWeek[i] = customFirstDay + i;
        }

        daysOfWeek[dayOfWeekOffset] = DayOfWeek.Sunday;
        int counter = 0;

        for (int i = dayOfWeekOffset; i < daysOfWeek.Length; i++)
        {
            daysOfWeek[i] = DayOfWeek.Sunday + counter;
            counter++;
        }
    }
}

void MarkHabitDone(string habitEntryID, bool habitDone = true)
{
    // @TODO: Handle marking future dates
    if (habitDone == false)
    {
        habitsCompleted.Remove(habitEntryID);
    }
    else
        habitsCompleted.Add(habitEntryID);

    SaveUserData();
    ShowWeekGrid();
}

int CalculateCurrentStreak(string habit)
{
    int currentStreak = 0;
    DateOnly currentHabitDate = DateOnly.FromDateTime(currentDate);
    string currentHabitID = $"{habit}{currentHabitDate}";

    while (habitsCompleted.Contains(currentHabitID))
    {
        currentHabitDate = currentHabitDate.AddDays(-1);
        currentHabitID = habit + currentHabitDate;
        currentStreak++;
    }

    return currentStreak;
}

int CalculateRecordStreak(string habit)
{
    int recordStreak = 0;

    foreach (string habitCompletedID in habitsCompleted)
    {
        DateOnly currentHabitDate;
        DateOnly.TryParse(habitCompletedID.Substring(habit.Length), out currentHabitDate);
        string currentHabitID = $"{habit}{currentHabitDate}";

        int tempRecordStreak = 0;

        if (habitCompletedID.ToString().Contains(habit))
        {
            while (habitsCompleted.Contains(currentHabitID))
            {
                currentHabitDate = currentHabitDate.AddDays(-1);
                currentHabitID = habit + currentHabitDate;
                tempRecordStreak++;
            }

            if (tempRecordStreak > recordStreak)
                recordStreak = tempRecordStreak;
        }
    }

    return recordStreak;
}

void ShowCustomDate(int year, int month, int day)
{
    DateTime customDate = new DateTime(year: year, month: month, day: day);
    selectedDate = customDate;
}