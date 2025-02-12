string[] habitsToTrack = ["Meditate", "Read", "Walk", "Code"];
DayOfWeek[] daysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };
List<string> habitsCompleted = new List<string>();
var dt = DateTime.Now;
string checkIcon = "- [x] ";
string uncheckIcon = "- [ ] ";
DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

// User input helper vriables
string? userInput;
bool exit = false;

do
{
    // Add setting for selecting first day of the week (or implement getting user locale setting).
    SetFirstDayOfWeek(firstDayOfWeek);
    // Display current / selected week habit streak na all time best streak
    ShowWeekGrid();
    // Present menu options
        // Show custom date
        // Add habit to list
        // Remove habit from list
        // Mark habit as completed
    MarkHabitDone("Walk2/12/2025");
    MarkHabitDone("Walk2/11/2025");
    // MarkHabitDone("Read2/16/2025");
    MarkHabitDone("Read2/12/2025", false);
    MarkHabitDone("Read2/11/2025");
    MarkHabitDone("Read2/10/2025");
    MarkHabitDone("Meditate2/12/2025");
    MarkHabitDone("Read2/10/2025");
    MarkHabitDone("Read2/11/2025");
    MarkHabitDone("Meditate2/14/2025");
    MarkHabitDone("Meditate2/12/2025");
    // MarkHabitDone("Read2/12/2025", false);

    userInput = Console.ReadLine();
    if (userInput != null && userInput.ToLower().Contains("exit"))
    {
        exit = true;
    }
}
while (exit == false);

void ShowWeekGrid()
{
    Console.WriteLine($"\n\t\tWelcome to Habit Tracker. Current date is {dt.DayOfWeek} {DateOnly.FromDateTime(dt)}.\n");

    // Grid Header
    string weekGridHeader = "";

    foreach (var day in daysOfWeek)
    {
        string currentWeekDay = $"{day.ToString()} ";

        weekGridHeader += currentWeekDay;
    }
    Console.WriteLine($"\t\t{weekGridHeader} Current Record\n");

    // Grid Body
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
                habitCheckRow += currentWeekDay;
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


    if (daysOfWeek[weekDay] == dt.DayOfWeek)
    {
        habitGridEntryDate = DateOnly.FromDateTime(dt);
    }

    else
    {
        dateDaysDifference = weekDay - Array.IndexOf(daysOfWeek, dt.DayOfWeek);
        DateTime habitEntryCalculatedDate = new DateTime();

        habitEntryCalculatedDate = dt.AddDays(dateDaysDifference);
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

    Console.Clear();
    ShowWeekGrid();
}

int CalculateCurrentStreak(string habit)
{
    int currentStreak = 0;
    DateOnly currentHabitDate = DateOnly.FromDateTime(dt);
    string currentHabitID = $"{habit}{currentHabitDate}";

    if (habitsCompleted.Contains(currentHabitID))
    {
        do
        {
            currentHabitDate = currentHabitDate.AddDays(-1);
            currentHabitID = habit + currentHabitDate;
            currentStreak++;
        } while (habitsCompleted.Contains(currentHabitID));
    }
    return currentStreak;
}

int CalculateRecordStreak(string habit)
{
    int recordStreak = 0;

    foreach (string currentCompletedHabit in habitsCompleted)
    {
        int tempRecordStreak = 0;
        DateOnly currentHabitDate;
        DateOnly.TryParse(currentCompletedHabit.Substring(habit.Length), out currentHabitDate);
        string currentHabitID = $"{habit}{currentHabitDate}";

        if (currentCompletedHabit.ToString().Contains(habit))
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