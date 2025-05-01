using Microsoft.VisualStudio.TestTools.UnitTesting;
using HabitTrackerLibrary;

namespace HabitTrackerLibrary.Tests;

[TestClass]
public class HabitTests
{
    [TestMethod]
    public void CalculateCurrentStreak_ShouldReturnCorrectStreak()
    {
        // Arrange
        var habit = new Habit
        {
            Name = "Exercise",
            CompletionDates = new List<DateOnly>
        {
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2))
        }
        };

        // Act
        int currentStreak = habit.CalculateCurrentStreak();

        // Assert
        Assert.AreEqual(3, currentStreak, "The current streak should be 3.");
    }
}
