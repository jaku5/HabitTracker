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
        [HttpGet]
        public ActionResult<List<HabitToTrack>> GetAll() =>
            HabitTrackerService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<HabitToTrack> Get(int id)
        {
            var habit = HabitTrackerService.Get(id);

            if (habit == null)
                return NotFound();

            return Ok(habit);
        }

        [HttpPost]
        public IActionResult Add(HabitToTrack habitToTrack)
        {
            if (!HabitTrackerService.IsValidHabitName(habitToTrack))
                return BadRequest("Invalid habit name.");

            HabitTrackerService.Add(habitToTrack);

            return CreatedAtAction(nameof(Get), new { id = habitToTrack.Id }, habitToTrack);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, HabitToTrack habitToTrack)
        {
            if (id != habitToTrack.Id)
                return BadRequest();

            var existingHabit = HabitTrackerService.Get(id);
            if (existingHabit is null)
                return NotFound();

            if (!HabitTrackerService.IsValidHabitName(habitToTrack))
                return BadRequest("Invalid habit name.");

            HabitTrackerService.Update(existingHabit, habitToTrack);

            return NoContent();
        }
    }
}
