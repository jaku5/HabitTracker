string[] habitsToTrack = ["Meditate", "Read", "Walk", "Code"];
DayOfWeek[] daysOfWeek = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
var dt = DateTime.Now;

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
    Console.WriteLine($"\t\t{weekGrid}\n");

    foreach (string habit in habitsToTrack)
    {
        Console.WriteLine($"{habit}\n");
    }
}
