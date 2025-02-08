string[] habitsToTrack = ["Meditate", "Read", "Walk", "Code"];

// User input helper vriables
string? userInput;
bool exit = false;


do
{
    Console.WriteLine("Welcome to habit tracker");
    // Present menu options
    // Show custom date
    // Add habit to list
    // Remove habit from list
    // Mark habit as completed


    userInput = Console.ReadLine();
    if (userInput.ToLower().Contains("exit"))
    {
        exit = true;
    }

    // Disaplay current week and habit status
    // Display current / selected week habit streak na all time best streak



}
while (exit == false);

