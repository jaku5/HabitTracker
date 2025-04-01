# Habit Tracker CLI

A simple command-line application to help you track your habits and monitor your progress over time.

## Features

- Add, remove, and rename habits.
- Mark habits as completed for specific dates.
- View a weekly grid of your habits and their completion status.
- Track your current and record streaks for each habit.
- Reorder habits position on the list.
- Customize the first day of the week for the weekly grid.

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download) or later installed on your system.

### Running the Program

1. Clone or download this repository to your local machine.
2. Open a terminal and navigate to the project directory.
3. Build the project using the following command:
   ```sh
   dotnet build
   ```
4. Run the program using:
   ```sh
   dotnet run
   ```

### Interacting with the Program

Once the program is running, you can interact with it using the following options:

1. **View Weekly Grid**  
   The program will display a weekly grid showing your habits and their completion status. Each day is represented with a checkbox:
   - `- [x]` indicates the habit was completed.
   - `- [ ]` indicates the habit was not completed.

2. **Access the Menu**  
   Type `menu` or `m` to open the options menu. From here, you can perform various actions.

### Menu Options

1. **Show week grid for selected date**  
   Displays the weekly grid for the currently selected date.

2. **Select date**  
   Allows you to choose a specific date to view or update habits. You will be prompted to enter the year, month, and day.

3. **Mark habit as done for selected date**  
   Select a habit and mark it as completed for the currently selected date. You can also unmark it if it was previously marked.

4. **Add or remove habit**  
   - To add a habit, type the name of the habit and press Enter.
   - To remove a habit, type the name of an existing habit. **Note:** Removing a habit will delete all associated tracking data.

5. **Rename an existing habit**  
   Select a habit from the list and provide a new name. This will update the habit name and preserve all associated tracking data.

6. **Reorder habit list**  
   - Enter the number corresponding to the habit you want to move.
   - Enter the new position for the selected habit (1 to the total number of habits).
   - The habit will be moved to the new position, and the updated order will be saved.

7. **Set first day of the week**  
   Customize the first day of the week for the weekly grid. Options include Monday, Tuesday, Wednesday, etc.

### Data Persistence

The program saves your habits and progress in a `habit_data.json` file. This file is stored in an operating system-specific application data folder. This file is automatically updated whenever you make changes.

### Exiting the Program

To exit the program, type `exit` in the grid view.

## Example Usage

1. Start the program:
   ```sh
   dotnet run
   ```
2. Add a new habit:
   - Type the name of the habit when prompted.
3. Mark a habit as completed:
   - Open the menu (`menu` or `m`).
   - Select option `3` and choose the habit to mark as done.
4. View your weekly progress:
   - The weekly grid will display your habits and their completion status.

## License

This project is licensed under the [Apache License 2.0](LICENSE).