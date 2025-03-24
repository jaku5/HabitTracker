DayOfWeek[] daysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };
DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

List<string> habitsToTrack = new List<string>();
List<string> habitsCompleted = new List<string>();

var currentDate = DateTime.Now;
var selectedDate = currentDate;

// User input helper variables
string? userInput;
bool validInput = false;
bool exit = false;

do
{
    LoadUserData();
    SetFirstDayOfWeek(firstDayOfWeek);
    ShowWeekGrid();

    userInput = Console.ReadLine();

    if (userInput != null && userInput.ToLower().Contains("exit"))
    {
        exit = true;
    }

    else if (userInput != null && (userInput.ToLower().Contains("menu") || userInput.ToLower().Contains('m')))
    {
        ShowMenu();
    }

} while (exit == false);

void ShowMenu()
{
    string menuSelection = "";
    validInput = false;

    do
    {
        Console.Clear();
        Console.WriteLine($"Select option (selected date is {selectedDate.DayOfWeek} {DateOnly.FromDateTime(selectedDate)}):\n");
        Console.WriteLine("1. Show week grid for selected date.");
        Console.WriteLine("2. Select date.");
        Console.WriteLine("3. Mark habit as done for selected date.");
        Console.WriteLine("4. Add or remove habit.");
        Console.WriteLine("5. Rename an existing habit.");
        Console.WriteLine("6. Set first day of the week");

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

                int year = currentDate.Year;
                int month = currentDate.Month;
                int day = currentDate.Day;

                do
                {
                    Console.WriteLine($"Type year number and press enter (today is {currentDate.Year}):");
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
                    Console.WriteLine($"Type month number and press enter (today is {currentDate.Month}):");
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
                    Console.WriteLine($"Type day number and press enter (today is {currentDate.Day}):");
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
                    Console.WriteLine($"Selected date: {DateOnly.FromDateTime(selectedDate)}. Press enter to continue.");
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
                // @TODO Add suport for unmark option
                if (DateOnly.FromDateTime(selectedDate) > DateOnly.FromDateTime(currentDate))
                {
                    Console.Clear();
                    Console.WriteLine($"Selected date {selectedDate} is in the future. Please try again with a valid date. Press enter to continue.");
                    Console.ReadLine();
                }

                else
                {
                    Console.WriteLine($"Type habit name you want to mark and press enter. Use \"false\" options to unmark habit done status. Selected date is ({selectedDate.DayOfWeek} {DateOnly.FromDateTime(selectedDate)}):");

                    userInput = Console.ReadLine();

                    if (userInput != null && habitsToTrack.Contains(userInput))
                    {
                        string habitsCompletedID = userInput + DateOnly.FromDateTime(selectedDate).ToString("yyyy-MM-dd");
                        MarkHabitDone(habitsCompletedID);
                        validInput = true;
                    }

                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Habit \"{userInput}\" is not on the list. Please enter a valid habit name or add a new habit to the list. Press enter continue.\n");
                        userInput = Console.ReadLine();
                    }
                }

                break;

            case "4":

                validInput = false;

                // @TODO Add warning and confimration before deleting data.s
                Console.Clear();
                Console.WriteLine($"Type habit name you want to add and press enter. Type exisitng habit name to delete it from the list. This deletes all habit track data as well.\n");
                userInput = Console.ReadLine();

                if (userInput != null && userInput != "" && !userInput.Contains(',') && !userInput.All(char.IsWhiteSpace))
                {
                    ModifyHabitList(userInput);
                    SaveUserData();
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

                if (habitsToTrack.Count == 0)
                {
                    Console.WriteLine("No habits to rename. Please add a habit first. Press enter to continue.");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("Select the habit you want to rename:");
                for (int i = 0; i < habitsToTrack.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {habitsToTrack[i]}");
                }

                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int renameIndex) && renameIndex > 0 && renameIndex <= habitsToTrack.Count)
                {
                    string oldHabitName = habitsToTrack[renameIndex - 1];
                    Console.WriteLine($"Enter the new name for the habit \"{oldHabitName}\":");
                    string? newHabitName = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newHabitName) && !newHabitName.Contains(','))
                    {
                        RenameHabit(oldHabitName, newHabitName);
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

                                    firstDayOfWeek = DayOfWeek.Monday;
                                    validInput = true;

                                    break;

                                case 2:

                                    firstDayOfWeek = DayOfWeek.Tuesday;
                                    validInput = true;

                                    break;

                                case 3:

                                    firstDayOfWeek = DayOfWeek.Wednesday;
                                    validInput = true;

                                    break;

                                case 4:

                                    firstDayOfWeek = DayOfWeek.Thursday;
                                    validInput = true;

                                    break;

                                case 5:

                                    firstDayOfWeek = DayOfWeek.Friday;
                                    validInput = true;

                                    break;

                                case 6:

                                    firstDayOfWeek = DayOfWeek.Saturday;
                                    validInput = true;

                                    break;

                                case 7:

                                    firstDayOfWeek = DayOfWeek.Sunday;
                                    validInput = true;

                                    break;
                            }
                        }
                    }

                } while (validInput == false);

                SetFirstDayOfWeek(firstDayOfWeek);

                Console.Clear();
                Console.WriteLine($"Selected first day of the week: {firstDayOfWeek}. Press Enter to continue.");
                Console.ReadLine();

                break;
        }

    } while (validInput == false);
}

void LoadUserData()
{
    if (File.Exists("./habit_data.txt"))
    {
        //@TODO Handle empty data file
        string habits = "";
        string habitsCompletedID = "";
        StreamReader sr = new StreamReader("./habit_data.txt");

        habits = sr.ReadLine();
        habitsCompletedID = sr.ReadLine();

        if (habits != "" && habits != null)
        {
            habitsToTrack = habits.Replace(", ", ",").Split(',').ToList();
        }

        else
        {
            sr.Close();

            do
            {
                Console.WriteLine("Welcome to Habit Tracker. To start tracking, add your first habit. Type the name of the habit you want to track and press enter:");

                userInput = Console.ReadLine();

                if (userInput != null && userInput != "" && !userInput.Contains(',') && !userInput.All(char.IsWhiteSpace))
                {
                    ModifyHabitList(userInput);
                    SaveUserData();
                }

                else
                {
                    Console.Clear();
                    Console.WriteLine($"Invalid habit name \"{userInput}\". Name cannot be empty and cannot contain a comma.\n");
                }

            } while (userInput == null || userInput == "" || userInput.Contains(',') || userInput.All(char.IsWhiteSpace));
        }

        if (habitsCompletedID != "" && habitsCompletedID != null)
        {
            habitsCompleted = habitsCompletedID.Replace(", ", ",").Split(',').ToList();
        }

        sr.Close();
    }

    else
    {
        do
        {
            Console.WriteLine("Welcome to Habit Tracker. To start tracking, add your first habit. Type the name of the habit you want to track and press enter:");

            userInput = Console.ReadLine();

            if (userInput != null && userInput != "" && !userInput.Contains(',') && !userInput.All(char.IsWhiteSpace))
            {
                ModifyHabitList(userInput);
                SaveUserData();
            }

            else
            {
                Console.Clear();
                Console.WriteLine($"Invalid habit name \"{userInput}\". Name cannot be empty and cannot contain a comma.\n");
            }

        } while (userInput == null || userInput == "" || userInput.Contains(',') || userInput.All(char.IsWhiteSpace));
    }
}

void SaveUserData()
{
    string userData = "";
    string habitList = "";
    string habitCompletedList = "";

    StreamWriter sw = new StreamWriter("./habit_data.txt");

    if (habitsToTrack.Count > 0)
    {
        foreach (string habit in habitsToTrack)
        {
            habitList += $"{habit}, ";
        }

        if (habitsCompleted.Count > 0)
        {
            foreach (string habitCompletedID in habitsCompleted)
            {
                habitCompletedList += $"{habitCompletedID}, ";
            }

            userData = $"{habitList.Remove(habitList.Length - 2)}\n{habitCompletedList.Remove(habitCompletedList.Length - 2)}";
        }

        else
        {
            userData = $"{habitList.Remove(habitList.Length - 2)}";
        }

        sw.WriteLine(userData);
        sw.Close();
    }

    else
    {
        sw.WriteLine(userData);
        sw.Close();
    }
}

void ModifyHabitList(string habit)
{
    if (habitsToTrack.Contains(habit))
    {
        for (int i = 0; i < habitsCompleted.Count; i++)
        {
            while (habitsCompleted.Count > 0 && habitsCompleted[i].ToString().Substring(0, habit.Length).Equals(habit))
            {
                MarkHabitDone(habitsCompleted[i], false);

                if (i > habitsCompleted.Count - 1) break;
            }
        }

        if (habitsToTrack.Count > 1)
        {
            habitsToTrack.Remove(habit);
        }

        else
        {
            habitsToTrack.Remove(habit);
            SaveUserData();
            LoadUserData();
        }
    }

    else
    {
        habitsToTrack.Add(habit);
    }

    SaveUserData();
    ShowWeekGrid();
}

void RenameHabit(string oldHabitName, string newHabitName)
{
    int habitIndex = habitsToTrack.IndexOf(oldHabitName);
    if (habitIndex != -1)
    {
        habitsToTrack[habitIndex] = newHabitName;
    }

    for (int i = 0; i < habitsCompleted.Count; i++)
    {
        if (habitsCompleted[i].StartsWith(oldHabitName))
        {
            string datePart = habitsCompleted[i].Substring(oldHabitName.Length);
            habitsCompleted[i] = newHabitName + datePart;
        }
    }

    SaveUserData();
}

void ShowWeekGrid()
{
    Console.Clear();
    Console.WriteLine($"\n\t\tWelcome to Habit Tracker. Today is {currentDate.DayOfWeek} {DateOnly.FromDateTime(currentDate)}.\n\t\tSelected date is {selectedDate.DayOfWeek} {DateOnly.FromDateTime(selectedDate)}. Type \"menu\" or \"m\" to display options menu.\n");

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
        string currentWeekDayDate = $"{CalculateGridDates(habitsToTrack[0], i).ToString("dd MMM")} ";

        if (i != 0)
            currentWeekDayDate = $"{CalculateGridDates(habitsToTrack[0], i).ToString("dd MMM").PadLeft(daysOfWeek[i - 1].ToString().Length)} ";

        weekGridHeader += currentWeekDay;
        weekGridHeaderDates += currentWeekDayDate;
    }
    Console.WriteLine($"\t\t{weekGridHeader} Current Record");
    Console.WriteLine($"\t\t{weekGridHeaderDates}\n");
}

void ShowGridBody()
{
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
            string checkIcon = "- [x] ";
            string uncheckIcon = "- [ ] ";
            string icon = uncheckIcon;
            habitEntryID = $"{habit}{habitDate.ToString("yyyy-MM-dd")}";

            if (habitsCompleted.Contains(habitEntryID))
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

                streaksRow = $"{currentStreak}".ToString().PadLeft(daysOfWeek[i].ToString().Length - 3) + $"{recordStreak}".ToString().PadLeft(8);
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

    if (daysOfWeek[weekDay] == selectedDate.DayOfWeek)
    {
        habitGridEntryDate = DateOnly.FromDateTime(selectedDate);
    }

    else
    {
        dateDaysDifference = weekDay - Array.IndexOf(daysOfWeek, selectedDate.DayOfWeek);
        DateTime habitEntryCalculatedDate = new DateTime();

        habitEntryCalculatedDate = selectedDate.AddDays(dateDaysDifference);
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
}

void MarkHabitDone(string habitEntryID, bool habitDone = true)
{
    if (habitDone == false)
        habitsCompleted.Remove(habitEntryID);

    else
        habitsCompleted.Add(habitEntryID);

    SaveUserData();
    ShowWeekGrid();
}

int CalculateCurrentStreak(string habit)
{
    int currentStreak = 0;
    DateOnly currentHabitDate = DateOnly.FromDateTime(currentDate);
    string currentHabitID = $"{habit}{currentHabitDate.ToString("yyyy-MM-dd")}";

    while (habitsCompleted.Contains(currentHabitID))
    {
        currentHabitDate = currentHabitDate.AddDays(-1);
        currentHabitID = habit + currentHabitDate.ToString("yyyy-MM-dd");
        currentStreak++;
    }

    return currentStreak;
}

int CalculateRecordStreak(string habit)
{
    int recordStreak = 0;

    foreach (string habitCompletedID in habitsCompleted)
    {
        DateOnly currentHabitDate;

        int tempRecordStreak = 0;

        if (habitCompletedID.ToString().Contains(habit))
        {
            DateOnly.TryParse(habitCompletedID.Substring(habit.Length), out currentHabitDate);
            string currentHabitID = $"{habit}{currentHabitDate.ToString("yyyy-MM-dd")}";

            while (habitsCompleted.Contains(currentHabitID))
            {
                currentHabitDate = currentHabitDate.AddDays(-1);
                currentHabitID = habit + currentHabitDate.ToString("yyyy-MM-dd");
                tempRecordStreak++;
            }

            if (tempRecordStreak > recordStreak)
                recordStreak = tempRecordStreak;
        }
    }

    return recordStreak;
}

void SetCustomDate(int year, int month, int day)
{
    DateTime customDate = new DateTime(year: year, month: month, day: day);

    selectedDate = customDate;
}