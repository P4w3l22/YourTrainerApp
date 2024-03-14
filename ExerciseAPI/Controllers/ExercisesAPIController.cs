using Azure;
using ExerciseAPI.Data;
using ExerciseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExerciseAPI.Controllers
{
    [Route("api/ExercisesAPI")]
    [ApiController]
    public class ExercisesAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ExercisesAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetExercises()
        {
            return Ok(_db.Exercises.ToList());
        }

        [HttpGet("{id:int}", Name = "GetExercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetExercise(int id)
        {
            if (id == 0)
            {
                ModelState.AddModelError("IdError", "Invalid Id!");
                return BadRequest(ModelState);
            }

            var exercise = _db.Exercises.FirstOrDefault(e => e.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Exercise> CreateExercise([FromBody]Exercise exercise)
        {
            if (exercise == null)
            {
                return BadRequest(exercise);
            }

            if (exercise.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            _db.Exercises.Add(exercise);
            _db.SaveChanges();

            return CreatedAtRoute("GetExercise", new { id = exercise.Id }, exercise);
        }

        [HttpDelete("{id:int}", Name = "DeleteExercise")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteExercise(int id)
        {
            if (id == 0) { return BadRequest(); }

            var exercise = _db.Exercises.FirstOrDefault(e => e.Id == id);

            if (exercise == null) { return NotFound(); }

            _db.Exercises.Remove(exercise);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateExercise")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateExercise(int id, [FromBody] Exercise exercise)
        {
            if (id == 0 || exercise == null || id != exercise.Id) { return BadRequest(); }

            _db.Exercises.Update(exercise);
            _db.SaveChanges();

            return CreatedAtRoute("GetExercise", new {id = exercise.Id}, exercise);
        }

        [HttpPatch("{id:int}", Name = "UpdateExerciseProperty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialView(int id, JsonPatchDocument<Exercise> patchExer)
        {
            if (id == 0 || patchExer == null) { return BadRequest(); }

            var exercise = _db.Exercises.AsNoTracking().FirstOrDefault(e => e.Id == id);

            if (exercise == null) { return BadRequest(); };

            patchExer.ApplyTo(exercise, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

            _db.Update(exercise);
            _db.SaveChanges();

            if (!ModelState.IsValid) { return BadRequest(); }

            return CreatedAtRoute("GetExercise", new { id = exercise.Id }, exercise);
        }
    }
}
