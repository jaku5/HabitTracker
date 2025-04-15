using Microsoft.AspNetCore.Mvc;
using HabitTrackerWebApp.Services;
using HabitTrackerWebApp.Models;

namespace HabitTrackerWebApp.Controllers
{
    [Route("[controller]")]
    public class HabitsController : Controller
    {
        public IActionResult Index()
        {
            var habits = HabitTrackerService.GetAll();
            return View(habits);
        }
    }
}