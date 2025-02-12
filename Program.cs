string[] habitsToTrack = ["Meditate", "Read", "Walk", "Code"];
DayOfWeek[] daysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };
var dt = DateTime.Now;
string checkIcon = "- [x] ";
string uncheckIcon = "- [ ] ";
DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

// User input helper vriables
string? userInput;
bool exit = false;

do
{
    Console.WriteLine($"\n\t\tWelcome to Habit Tracker. Current date is {dt.DayOfWeek} {DateOnly.FromDateTime(dt)}.\n");
    // Add setting for selecting first day of the week (or implement getting user locale setting).
    SetFirstDayOfWeek(firstDayOfWeek);
    // Disaplay current week and habit status
    // Display current / selected week habit streak na all time best streak
    ShowWeekGrid();
    // Present menu options
    // Show custom date
    // Add habit to list
    // Remove habit from list
    // Mark habit as completed
    Console.WriteLine();

    userInput = Console.ReadLine();
    if (userInput != null && userInput.ToLower().Contains("exit"))
    {
        exit = true;
    }
}
while (exit == false);

void ShowWeekGrid()
{
    SetFirstDayOfWeek(firstDayOfWeek);
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
        string habitCheckRow = "";
        string streaksRow = "";
        string habitEntryDate = "";

        for (int i = 0; i < daysOfWeek.Length; i++)
        {
            string currentWeekDay = "";
            // habitEntryDate = CalculateGridDates(habit, i).Day.ToString();
            habitEntryDate = "";

            if (i == 0)
            {
                currentWeekDay = uncheckIcon;
                habitCheckRow += currentWeekDay + habitEntryDate;
            }

            else if (i == daysOfWeek.Length - 1)
            {
                int currentStreak = 0;
                int recordStreak = 0;

                currentWeekDay = $"{uncheckIcon}".PadLeft(daysOfWeek[i - 1].ToString().Length + 1);
                habitCheckRow += currentWeekDay + habitEntryDate;

                streaksRow = $"{currentStreak}".ToString().PadLeft(daysOfWeek[i].ToString().Length - 3) + $"{recordStreak}".ToString().PadLeft(8);
            }

            else
            {
                currentWeekDay = $"{uncheckIcon}".PadLeft(daysOfWeek[i - 1].ToString().Length + 1);
                habitCheckRow += currentWeekDay + habitEntryDate;
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