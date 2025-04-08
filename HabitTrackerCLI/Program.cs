using System.Text.Json;
using HabitTrackerLibrary;

try
{
    string userDataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HabitTrackerCLI", "habit_data.json");
    string? userDirectoryPath = Path.GetDirectoryName(userDataFilePath);

    if (!string.IsNullOrEmpty(userDirectoryPath) && !Directory.Exists(userDirectoryPath))
    {
        Directory.CreateDirectory(userDirectoryPath);
    }

    HabitTracker habitTracker = new HabitTracker(userDataFilePath);
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

catch (IOException ex) when (ex.InnerException is IOException)
{
    Console.Clear();
    Console.Write($"Error: {ex.Message} ");
    Console.WriteLine("I/O error occurred while saving user data. Please restart application and try again.");
    Console.WriteLine("Press enter to exit.");
    Console.ReadLine();
}

catch (UnauthorizedAccessException ex) when (ex.InnerException is UnauthorizedAccessException)
{
    Console.Clear();
    Console.Write($"Error: {ex.Message} ");
    Console.WriteLine("Access denied while saving user data. Please ensure you have necessary permission before attempting saving data.");
    Console.WriteLine("Press enter to exit.");
    Console.ReadLine();
}