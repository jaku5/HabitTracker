using Microsoft.AspNetCore.Mvc;
using HabitTrackerWebApp.Services;

namespace HabitTrackerWebApp.Controllers
{
    [Route("[controller]")]
    public class HabitsController : Controller
    {
        public IActionResult Index(DateTime? selectedDate, DayOfWeek? firstDayOfWeek)
        {
            var habits = HabitTrackerService.GetAll();
            ViewBag.SelectedDate = selectedDate ?? DateTime.Now;
            ViewBag.FirstDayOfWeek = firstDayOfWeek ?? DayOfWeek.Monday;

            var startOfWeek = ((DateTime)ViewBag.SelectedDate).Date;
            while (startOfWeek.DayOfWeek != ViewBag.FirstDayOfWeek)
            {
                startOfWeek = startOfWeek.AddDays(-1);
            }
            ViewBag.WeekRange = Enumerable.Range(0, 7).Select(offset => startOfWeek.AddDays(offset)).ToList();

            return View(habits);
        }
    }
}