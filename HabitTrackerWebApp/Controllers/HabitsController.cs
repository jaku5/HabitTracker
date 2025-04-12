using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HabitTrackerWebApp.Services;
using HabitTrackerWebApp.Models;

namespace HabitTrackerWebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HabitsController : ControllerBase
    {
        public HabitsController()
        {
            HabitTrackerService.Initialize();
        }

        [HttpGet]
        public ActionResult<List<HabitToTrack>> GetAll() =>
            HabitTrackerService.GetAll();

        [HttpGet("{name}")]
        public ActionResult<HabitToTrack> Get(string name)
        {
            var habit = HabitTrackerService.Get(name);

            if (habit == null)
                return NotFound();

            return Ok(habit);
        }

        [HttpPost]
        public IActionResult Add(HabitToTrack habitToTrack)
        {
            if (!HabitTrackerService.IsValidHabitName(habitToTrack))
                return BadRequest("Invalid habit name.");

            var newHabit = new HabitToTrack { Name = habitToTrack.Name };
            HabitTrackerService.Add(newHabit);

            return CreatedAtAction(nameof(Get), new { name = newHabit.Name }, newHabit);
        }
    }
}
