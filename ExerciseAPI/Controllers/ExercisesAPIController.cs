using Azure;
using ExerciseAPI.Data;
using ExerciseAPI.Models;
using ExerciseAPI.Repository;
using ExerciseAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ExerciseAPI.Controllers
{
    [Route("api/ExercisesAPI")]
    [ApiController]
    public class ExercisesAPIController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseDb;
        protected APIResponse _response;
        public ExercisesAPIController(IExerciseRepository exerciseDb)
        {
			_exerciseDb = exerciseDb;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetExercises()
        {
            try
            {
                IEnumerable<Exercise> exerList = await _exerciseDb.GetAllAsync();
                _response.Result = exerList;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetExercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetExercise(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var exercise = await _exerciseDb.GetAsync(e => e.Id == id);

                if (exercise == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                return Ok(exercise);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateExercise([FromBody]Exercise exercise)
        {
            try
            {
                if (await _exerciseDb.GetAsync(u => u.Name.ToLower() == exercise.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Ćwiczenie już istnieje!");
                    return BadRequest(ModelState);
                }

                if (exercise == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(exercise);
                }

                if (exercise.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                _exerciseDb.CreateAsync(exercise);
                return CreatedAtRoute("GetExercise", new { id = exercise.Id }, exercise);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;

        }

        [HttpDelete("{id:int}", Name = "DeleteExercise")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteExercise(int id)
        {
            try
            {
                if (id == 0) { return BadRequest(); }

                var exercise = await _exerciseDb.GetAsync(e => e.Id == id);

                if (exercise == null) { return NotFound(); }

                await _exerciseDb.RemoveAsync(exercise);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateExercise")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateExercise(int id, [FromBody] Exercise exercise)
        {
            try
            {
                if (id == 0 || exercise == null || id != exercise.Id) 
                {
                    ModelState.AddModelError("IdError", "Niewłaściwe id!");
                    return BadRequest(ModelState); 
                
                }

                await _exerciseDb.UpdateAsync(exercise);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
            
            
        }

        //[HttpPatch("{id:int}", Name = "UpdateExerciseProperty")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult UpdatePartialView(int id, JsonPatchDocument<Exercise> patchExer)
        //{
        //    if (id == 0 || patchExer == null) { return BadRequest(); }

        //    var exercise = _db.Exercises.AsNoTracking().FirstOrDefault(e => e.Id == id);

        //    if (exercise == null) { return BadRequest(); };

        //    patchExer.ApplyTo(exercise, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

        //    _db.Update(exercise);
        //    _db.SaveChanges();

        //    if (!ModelState.IsValid) { return BadRequest(); }

        //    return CreatedAtRoute("GetExercise", new { id = exercise.Id }, exercise);
        //}
    }
}
