string[] habitsToTrack = ["Meditate", "Read", "Walk", "Code"];
DayOfWeek[] daysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };
var dt = DateTime.Now;
string checkIcon = "- [x] ";
string uncheckIcon = "- [ ] ";

// User input helper vriables
string? userInput;
bool exit = false;

do
{
    Console.WriteLine($"\n\t\tWelcome to Habit Tracker. Current date is {dt.DayOfWeek} {DateOnly.FromDateTime(dt)}.\n");
    // Present menu options
    // Show custom date
    ShowWeekGrid();
    // Add habit to list
    // Remove habit from list
    // Mark habit as completed

    Console.WriteLine();

    userInput = Console.ReadLine();
    if (userInput != null && userInput.ToLower().Contains("exit"))
    {
        exit = true;
    }

    // Disaplay current week and habit status
    // Display current / selected week habit streak na all time best streak
    // Add setting for selecting first day of the week (or implement getting user locale setting).

}
while (exit == false);

void ShowWeekGrid()
{
    string weekGrid = "";

    foreach (var day in daysOfWeek)
    {
        string currentWeekDay = $"{day.ToString()} ";

        weekGrid += currentWeekDay;
    }
    Console.WriteLine($"\t\t{weekGrid} Current Record\n");

    foreach (string habit in habitsToTrack)
    {
        string habitCheckRow = "";
        string streaksRow = "";
        string habitEntryDate = "";

        for (int i = 0; i < daysOfWeek.Length; i++)
        {
            string currentWeekDay = "";
            habitEntryDate = CalculateGridDate(habit, i).ToString();

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

DateOnly CalculateGridDate(string habit, int weekDay)
{
    DateOnly habitEntryDate = new DateOnly();
    int dateDaysDifference = 0;

    // Custom start of week day calculations
    // if (weekDay > 0)
    //     weekDay--;
    // else
    //     weekDay = 6;


    if (daysOfWeek[weekDay] == dt.DayOfWeek)
    {
        habitEntryDate = DateOnly.FromDateTime(dt);
    }

    else
    {
        dateDaysDifference = (int)(weekDay - dt.DayOfWeek);
        DateTime habitEntryCalculatedDate = new DateTime();

        habitEntryCalculatedDate = dt.AddDays(dateDaysDifference);
        habitEntryDate = DateOnly.FromDateTime(habitEntryCalculatedDate);
    }


    return habitEntryDate;
}