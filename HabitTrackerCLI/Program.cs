using System.Text.Json;
using HabitTrackerCLI;

try
{
    HabitTracker habitTracker = new HabitTracker();
    UserInterface userInterface = new UserInterface(habitTracker);

    do
    {
        if (habitTracker.LoadUserData())
            userInterface.ShowWeekGrid();
        else
            userInterface.InitializeUserData();

        userInterface.userInput = Console.ReadLine();

        if (userInterface.userInput != null && userInterface.userInput.ToLower().Contains("exit"))
            userInterface.exit = true;

        else if (userInterface.userInput != null && (userInterface.userInput.ToLower().Contains("menu") || userInterface.userInput.ToLower().Contains('m')))
            userInterface.ShowMenu();

    } while (userInterface.exit == false);
}

catch (InvalidDataException ex) when (ex.InnerException is JsonException)
{
    Console.Clear();
    Console.Write($"Error: {ex.Message} ");
    Console.WriteLine("Please inspect the file or delete it for a fresh start.");
    Console.WriteLine("Press enter to exit.");
    Console.ReadLine();
}
