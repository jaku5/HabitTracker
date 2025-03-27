namespace HabitTrackerCLI;

public class UserInterface
{
    private readonly HabitTracker habitTracker;

    public UserInterface(HabitTracker habitTracker)
    {
        this.habitTracker = habitTracker;
    }

    DayOfWeek[] daysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };

    // User input helper properties
    public string? userInput { get; set; }
    bool validInput = false;
    internal bool exit  { get; set; } = false;

    public void ShowMenu()
    {
        string menuSelection = "";
        validInput = false;

        do
        {
            Console.Clear();
            Console.WriteLine($"Select option (selected date is {habitTracker.SelectedDate.DayOfWeek} {DateOnly.FromDateTime(habitTracker.SelectedDate)}):\n");
            Console.WriteLine("1. Show week grid for selected date.");
            Console.WriteLine("2. Select date.");
            Console.WriteLine("3. Mark habit as done for selected date.");
            Console.WriteLine("4. Add or remove habit.");
            Console.WriteLine("5. Rename an existing habit.");
            Console.WriteLine("6. Reorder habit list.");
            Console.WriteLine("7. Set first day of the week.");

            userInput = Console.ReadLine();

            if (userInput != null)
            {
                menuSelection = userInput.ToLower();
            }

            switch (menuSelection)
            {
                case "1":

                    validInput = true;

                    ShowWeekGrid();

                    break;

                case "2":

                    Console.Clear();

                    validInput = false;

                    int year = habitTracker.CurrentDate.Year;
                    int month = habitTracker.CurrentDate.Month;
                    int day = habitTracker.CurrentDate.Day;

                    do
                    {
                        Console.WriteLine($"Type year number and press enter (today is {habitTracker.CurrentDate.Year}):");
                        userInput = Console.ReadLine();

                        if (userInput != null)
                        {
                            validInput = int.TryParse(userInput, out year) && year > 0 && year <= 9999;
                            if (validInput == true) break;
                        }

                        Console.Clear();
                        Console.WriteLine($"Invalid year \"{year}\". Year must be a number between 1 and 9999.\n");

                    } while (validInput == false);

                    do
                    {
                        Console.WriteLine($"Type month number and press enter (today is {habitTracker.CurrentDate.Month}):");
                        userInput = Console.ReadLine();

                        if (userInput != null)
                        {
                            validInput = int.TryParse(userInput, out month) && month > 0 && month <= 12;
                            if (validInput == true) break;
                        }

                        Console.Clear();
                        Console.WriteLine($"Invalid month \"{month}\". Month must be a number between 1 and 12.\n");

                    } while (validInput == false);

                    do
                    {
                        Console.WriteLine($"Type day number and press enter (today is {habitTracker.CurrentDate.Day}):");
                        userInput = Console.ReadLine();

                        if (userInput != null)
                        {
                            validInput = int.TryParse(userInput, out day) && month > 0 && month <= 31;
                            if (validInput == true) break;
                        }

                        Console.Clear();
                        Console.WriteLine($"Invalid day \"{day}\". Day must be a number between 1 and 31.\n");

                    } while (validInput == false);

                    try
                    {
                        SetCustomDate(year, month, day);
                        Console.Clear();
                        Console.WriteLine($"Selected date: {DateOnly.FromDateTime(habitTracker.SelectedDate)}. Press enter to continue.");
                        Console.ReadLine();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.Clear();
                        Console.WriteLine($"Selected date does not seem to be valid: \"{e.Message}\" Please try again and make sure to enter a valid date. Press enter continue.");
                        Console.ReadLine();
                    }

                    break;

                case "3":

                    validInput = false;

                    Console.Clear();

                    if (DateOnly.FromDateTime(habitTracker.SelectedDate) > DateOnly.FromDateTime(habitTracker.CurrentDate))
                    {
                        Console.Clear();
                        Console.WriteLine($"Selected date {habitTracker.SelectedDate} is in the future. Please try again with a valid date. Press enter to continue.");
                        Console.ReadLine();
                    }

                    else
                    {
                        for (int i = 0; i < habitTracker.HabitsToTrack.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {habitTracker.HabitsToTrack[i]}");
                        }

                        Console.WriteLine($"\nEnter the number corresponding to the habit (selected date is {habitTracker.SelectedDate.DayOfWeek} {DateOnly.FromDateTime(habitTracker.SelectedDate)}):");
                        userInput = Console.ReadLine();

                        if (int.TryParse(userInput, out int habitIndex) && habitIndex > 0 && habitIndex <= habitTracker.HabitsToTrack.Count)
                        {
                            string selectedHabit = habitTracker.HabitsToTrack[habitIndex - 1];
                            string habitsCompletedID = selectedHabit + DateOnly.FromDateTime(habitTracker.SelectedDate).ToString("yyyy-MM-dd");

                            habitTracker.MarkHabitDone(habitsCompletedID);
                            validInput = true;
                        }

                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Invalid selection. Please enter a valid number corresponding to a habit. Press enter to continue.");
                            Console.ReadLine();
                        }
                    }

                    break;

                case "4":

                    validInput = false;

                    // @TODO Add warning and confimration before deleting data
                    Console.Clear();
                    Console.WriteLine($"Type habit name you want to add and press enter. Type exisitng habit name to delete it from the list. This deletes all habit track data as well.\n");
                    userInput = Console.ReadLine();

                    if (userInput != null && userInput != "" && !userInput.Contains(',') && !userInput.All(char.IsWhiteSpace))
                    {
                        habitTracker.ModifyHabitList(userInput);
                        habitTracker.SaveUserData();
                        validInput = true;
                    }

                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Invalid habit name \"{userInput}\". Name cannot be empty and cannot contain a comma. Press enter continue.\n");
                        Console.ReadLine();
                    }

                    break;

                case "5":

                    validInput = false;
                    Console.Clear();

                    if (habitTracker.HabitsToTrack.Count == 0)
                    {
                        Console.WriteLine("No habits to rename. Please add a habit first. Press enter to continue.");
                        Console.ReadLine();
                        InitializeUserData();
                        break;
                    }

                    Console.WriteLine("Select the habit you want to rename:");
                    for (int i = 0; i < habitTracker.HabitsToTrack.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {habitTracker.HabitsToTrack[i]}");
                    }

                    userInput = Console.ReadLine();

                    if (int.TryParse(userInput, out int renameIndex) && renameIndex > 0 && renameIndex <= habitTracker.HabitsToTrack.Count)
                    {
                        string oldHabitName = habitTracker.HabitsToTrack[renameIndex - 1];
                        Console.WriteLine($"Enter the new name for the habit \"{oldHabitName}\":");
                        string? newHabitName = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(newHabitName) && !newHabitName.Contains(','))
                        {
                            habitTracker.RenameHabit(oldHabitName, newHabitName);
                            Console.WriteLine($"Habit \"{oldHabitName}\" has been renamed to \"{newHabitName}\". Press enter to continue.");
                            Console.ReadLine();
                        }

                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"Invalid habit name \"{userInput}\". Name cannot be empty and cannot contain a comma. Press enter continue.\n");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. Press enter to continue.");
                        Console.ReadLine();
                    }

                    break;

                case "6":

                    validInput = false;
                    Console.Clear();

                    if (habitTracker.HabitsToTrack.Count == 0)
                    {
                        Console.WriteLine("No habits to reorder. Please add a habit first. Press enter to continue.");
                        Console.ReadLine();
                        InitializeUserData();
                        break;
                    }

                    Console.WriteLine("Select the habit you want to reorder:");
                    for (int i = 0; i < habitTracker.HabitsToTrack.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {habitTracker.HabitsToTrack[i]}");
                    }

                    userInput = Console.ReadLine();

                    if (int.TryParse(userInput, out int reorderIndex) && reorderIndex > 0 && reorderIndex <= habitTracker.HabitsToTrack.Count)
                    {
                        string habitName = habitTracker.HabitsToTrack[reorderIndex - 1];

                        Console.WriteLine($"Enter the new position for the habit \"{habitName}\":");
                        string? newHabitListPosition = Console.ReadLine();

                        if (int.TryParse(newHabitListPosition, out int newHabitIndex) && newHabitIndex > 0 && newHabitIndex <= habitTracker.HabitsToTrack.Count)
                        {
                            habitTracker.HabitsToTrack.RemoveAt(reorderIndex - 1);
                            habitTracker.HabitsToTrack.Insert(newHabitIndex - 1, habitName);

                            Console.WriteLine($"Habit \"{habitName}\" has been moved to position \"{newHabitIndex}\". Press enter to continue.");
                            habitTracker.SaveUserData();
                            Console.ReadLine();
                        }

                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"Invalid position \"{newHabitListPosition}\". Please enter a number between 1 and {habitTracker.HabitsToTrack.Count}. Press enter to continue.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. Press enter to continue.");
                        Console.ReadLine();
                    }

                    break;

                case "7":

                    validInput = false;
                    int selectedFirstDay = 0;

                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Set first day of the week for the week grid:\n\n1. Monday\n2. Tuesday\n3. Wednesday\n4. Thursday\n5. Friday\n6. Saturday\n7. Sunday");
                        userInput = Console.ReadLine();

                        if (int.TryParse(userInput, out selectedFirstDay))
                        {

                            {
                                switch (selectedFirstDay)
                                {
                                    case 1:

                                        habitTracker.FirstDayOfWeek = DayOfWeek.Monday;
                                        validInput = true;

                                        break;

                                    case 2:

                                        habitTracker.FirstDayOfWeek = DayOfWeek.Tuesday;
                                        validInput = true;

                                        break;

                                    case 3:

                                        habitTracker.FirstDayOfWeek = DayOfWeek.Wednesday;
                                        validInput = true;

                                        break;

                                    case 4:

                                        habitTracker.FirstDayOfWeek = DayOfWeek.Thursday;
                                        validInput = true;

                                        break;

                                    case 5:

                                        habitTracker.FirstDayOfWeek = DayOfWeek.Friday;
                                        validInput = true;

                                        break;

                                    case 6:

                                        habitTracker.FirstDayOfWeek = DayOfWeek.Saturday;
                                        validInput = true;

                                        break;

                                    case 7:

                                        habitTracker.FirstDayOfWeek = DayOfWeek.Sunday;
                                        validInput = true;

                                        break;
                                }
                            }
                        }

                    } while (validInput == false);

                    SetFirstDayOfWeek(habitTracker.FirstDayOfWeek);

                    Console.Clear();
                    Console.WriteLine($"Selected first day of the week: {habitTracker.FirstDayOfWeek}. Press Enter to continue.");
                    Console.ReadLine();

                    break;
            }

        } while (validInput == false);
    }

    public void InitializeUserData()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("Welcome to Habit Tracker. To start tracking, add your first habit. Type the name of the habit you want to track and press enter:");

            userInput = Console.ReadLine();

            if (userInput != null && userInput != "" && !userInput.Contains(',') && !userInput.All(char.IsWhiteSpace))
            {
                habitTracker.ModifyHabitList(userInput);
                habitTracker.SaveUserData();
                ShowWeekGrid();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Invalid habit name \"{userInput}\". Name cannot be empty and cannot contain a comma.\n");
            }

        } while (userInput == null || userInput == "" || userInput.Contains(',') || userInput.All(char.IsWhiteSpace));
    }

    public void ShowWeekGrid()
    {
        Console.Clear();
        Console.WriteLine($"\n\t\tWelcome to Habit Tracker. Today is {habitTracker.CurrentDate.DayOfWeek} {DateOnly.FromDateTime(habitTracker.CurrentDate)}.\n\t\tSelected date is {habitTracker.SelectedDate.DayOfWeek} {DateOnly.FromDateTime(habitTracker.SelectedDate)}. Type \"menu\" or \"m\" to display options menu.\n");

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
            string currentWeekDayDate = $"{CalculateGridDates(habitTracker.HabitsToTrack[0], i).ToString("dd MMM")} ";

            if (i != 0)
                currentWeekDayDate = $"{CalculateGridDates(habitTracker.HabitsToTrack[0], i).ToString("dd MMM").PadLeft(daysOfWeek[i - 1].ToString().Length)} ";

            weekGridHeader += currentWeekDay;
            weekGridHeaderDates += currentWeekDayDate;
        }
        Console.WriteLine($"\t\t{weekGridHeader} Current Record");
        Console.WriteLine($"\t\t{weekGridHeaderDates}\n");
    }

    void ShowGridBody()
    {
        foreach (string habit in habitTracker.HabitsToTrack)
        {
            int currentStreak = habitTracker.CalculateCurrentStreak(habit);
            int recordStreak = habitTracker.CalculateRecordStreak(habit);

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
                habitEntryID = $"{habit}{habitDate.ToString("yyyy-MM-dd")}";

                if (habitTracker.HabitsCompleted.Contains(habitEntryID))
                {
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

                    int currentStreakPadding = daysOfWeek[i].ToString().Length + currentStreak.ToString().Length - 4;
                    int recordStreakPadding = 8 + recordStreak.ToString().Length - currentStreak.ToString().Length;

                    streaksRow = currentStreak.ToString().PadLeft(currentStreakPadding) + recordStreak.ToString().PadLeft(recordStreakPadding);
                }

                else
                {
                    currentWeekDay = $"{icon}".PadLeft(daysOfWeek[i - 1].ToString().Length + 1);
                    habitCheckRow += currentWeekDay;
                }
            }

            if (habit.Length > 14)
            {
                FormatLongHabitName(habit, habitCheckRow, streaksRow);
            }

            else
            {
                Console.Write($"{habit}");
                Console.WriteLine($"{habitCheckRow}".PadLeft(habitCheckRow.Length + (16 - habit.Length)) + $"{streaksRow}\n");
            }
        }
    }

    void FormatLongHabitName(string habit, string habitCheckRow, string streaksRow)
    {
        string longHabit = "";
        int padLength = 0;
        int lineLength = 0;

        if (habit.Contains(' '))
        {
            string[] longHabitNameParts = habit.Split(' ');

            for (int i = 0; i < longHabitNameParts.Length; i++)
            {

                if (i == longHabitNameParts.Length - 1 && longHabitNameParts[i].Length < 14)
                {
                    if (lineLength + longHabitNameParts[i].Length < 14)
                    {
                        longHabit += $"{longHabitNameParts[i]}";
                        lineLength += longHabitNameParts[i].Length;
                        padLength = lineLength;
                    }

                    else
                    {
                        if (longHabit.LastIndexOf('\n') == longHabit.Length - 1)
                            longHabit += $"{longHabitNameParts[i]}";

                        else
                            longHabit += $"\n{longHabitNameParts[i]}";

                        lineLength = longHabitNameParts[i].Length;
                        padLength = lineLength;
                    }
                }

                else if (longHabitNameParts[i].Length < 14)
                {
                    if (lineLength + longHabitNameParts[i].Length < 14)
                    {
                        longHabit += $"{longHabitNameParts[i]} ";
                        lineLength += longHabitNameParts[i].Length + 1;
                    }

                    else
                    {
                        if (longHabit.LastIndexOf('\n') == longHabit.Length - 1)
                            longHabit += $"{longHabitNameParts[i]} ";

                        else
                            longHabit += $"\n{longHabitNameParts[i]} ";

                        lineLength = longHabitNameParts[i].Length + 1;
                    }
                }

                else
                {
                    char[] longHabitName = longHabitNameParts[i].ToCharArray();
                    string longHabitTemp = "";
                    string longHabitPart = "";

                    int charCounter = 0;

                    do
                    {
                        longHabitTemp = "";

                        while (longHabitTemp.Length <= 14 && charCounter < longHabitNameParts[i].Length)
                        {
                            longHabitTemp += $"{longHabitName[charCounter]}";
                            charCounter++;
                        }

                        longHabitPart += $"{longHabitTemp}\n";

                    } while (longHabitPart.Length < longHabitNameParts[i].Length);

                    lineLength = longHabitPart.Length;
                    longHabit += $"\n{longHabitPart}";

                    if (i == longHabitNameParts.Length - 1)
                    {
                        padLength = longHabitTemp.Length;
                    }
                }
            }

            if (longHabit.LastIndexOf('\n') == longHabit.Length - 1)
                Console.Write(longHabit.Remove(longHabit.Length - 1, 1));

            else
                Console.Write(longHabit);

            Console.WriteLine($"{habitCheckRow}".PadLeft(habitCheckRow.Length + (16 - padLength)) + $"{streaksRow}\n");
        }

        else
        {
            char[] longHabitName = habit.ToCharArray();
            string longHabitTemp = "";
            int charCounter = 0;

            do
            {
                longHabitTemp = "";

                while (longHabitTemp.Length <= 14 && charCounter < habit.Length)
                {
                    longHabitTemp += $"{longHabitName[charCounter]}";
                    charCounter++;
                }

                longHabit += $"{longHabitTemp}\n";
            } while (longHabit.Length < habit.Length);

            Console.Write(longHabit.Remove(longHabit.Length - 1, 1));
            Console.WriteLine($"{habitCheckRow}".PadLeft(habitCheckRow.Length + (16 - longHabitTemp.Length)) + $"{streaksRow}\n");
        }
    }

    DateOnly CalculateGridDates(string habit, int weekDay)
    {
        DateOnly habitGridEntryDate = new DateOnly();
        int dateDaysDifference = 0;

        if (daysOfWeek[weekDay] == habitTracker.SelectedDate.DayOfWeek)
        {
            habitGridEntryDate = DateOnly.FromDateTime(habitTracker.SelectedDate);
        }

        else
        {
            dateDaysDifference = weekDay - Array.IndexOf(daysOfWeek, habitTracker.SelectedDate.DayOfWeek);
            DateTime habitEntryCalculatedDate = new DateTime();

            habitEntryCalculatedDate = habitTracker.SelectedDate.AddDays(dateDaysDifference);
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

        else
        {
            Array.Sort(daysOfWeek);
        }

        habitTracker.SaveUserData();
    }

    void SetCustomDate(int year, int month, int day)
    {
        DateTime customDate = new DateTime(year: year, month: month, day: day);

        habitTracker.SelectedDate = customDate;
    }
}
