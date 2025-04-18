using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HabitTrackerWebApp.Services;
using HabitTrackerWebApp.Models;

namespace HabitTrackerWebApp.Controllers
{
    [Route("api/habits")]
    [ApiController]
    public class HabitsApiController : ControllerBase
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

        [HttpPost("{id}/mark-completed")]
        public IActionResult MarkHabitDone(int id, [FromBody] MarkHabitDoneRequest request)
        {
                if (request == null || string.IsNullOrWhiteSpace(request.Date))
        return BadRequest("Request body is missing or invalid.");

            var habit = HabitTrackerService.Get(id);
            if (habit is null)
                return NotFound();

            if (!DateOnly.TryParse(request.Date, out DateOnly completionDate))
                return BadRequest("Invalid date format.");

            HabitTrackerService.MarkHabitDone(habit, completionDate);

            return NoContent();
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var habit = HabitTrackerService.Get(id);

            if (habit is null)
                return NotFound();

            HabitTrackerService.Delete(id);

            return NoContent();
        }

        public class MarkHabitDoneRequest
        {
            public string Date { get; set; }
        }
    }
}
