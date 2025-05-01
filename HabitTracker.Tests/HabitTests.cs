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

    [TestMethod]
    public void CalculateRecordStreak_ShouldReturnCorrectStreak()
    {
        // Arrange
        var fixedDate = new DateTime(2025, 5, 1);
        var habit = new Habit
        {
            Name = "Exercise",
            CompletionDates = new List<DateOnly>
        {
            DateOnly.FromDateTime(fixedDate.AddDays(-1)),
            DateOnly.FromDateTime(fixedDate.AddDays(-2)),
            DateOnly.FromDateTime(fixedDate.AddDays(-3))
        }
        };

        // Act
        int recordStreak = habit.CalculateRecordStreak();

        // Assert
        Assert.AreEqual(3, recordStreak, "The record streak should be 3.");
    }
}
