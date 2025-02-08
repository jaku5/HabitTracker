string[] habitsToTrack = ["Meditate", "Read", "Walk", "Code"];

// User input helper vriables
string? userInput;
bool exit = false;


do
{
    Console.WriteLine("Welcome to habit tracker");
    userInput = Console.ReadLine();
    if (userInput.ToLower().Contains("exit"))
    {
        exit = true;
    }
    // Disaplay current week and habit status

}
while (exit == false);

