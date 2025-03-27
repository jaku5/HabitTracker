using HabitTrackerCLI;

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
    {
        userInterface.exit = true;
    }

    else if (userInterface.userInput != null && (userInterface.userInput.ToLower().Contains("menu") || userInterface.userInput.ToLower().Contains('m')))
    {
        userInterface.ShowMenu();
    }

} while (userInterface.exit == false);