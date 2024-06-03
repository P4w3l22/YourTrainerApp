using AutoMapper;
using DbDataAccess.Data;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExerciseAPI.Controllers;

[Route("api/ExerciseAPI")]
[ApiController]
public class ExercisesAPIController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IExerciseData _data;
    protected APIResponse _response;
    public ExercisesAPIController(IMapper mapper, IExerciseData data)
    {
        _mapper = mapper;
        _data = data;
        this._response = new();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetExercises()
    {
        try
        {
            IEnumerable<ExerciseModel> exerList = await _data.GetExercises();
            _response.Result = _mapper.Map<List<ExerciseDTO>>(exerList);
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

    //[Authorize(Roles = "admin")]
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

            var exercise = await _data.GetExercise(id);

            if (exercise is null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }
            _response.Result = _mapper.Map<ExerciseDTO>(exercise);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Errors = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpPost]
	[Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateExercise([FromBody]ExerciseCreateDTO exerciseCreate)
    {
        try
        {
            var exercises = await _data.GetExercises();
            var exList = exercises.Where(e => e.Name.ToLower() == exerciseCreate.Name.ToLower()).ToList();
            if (exList.Count > 0)
            {
                ModelState.AddModelError("Errors", "Ćwiczenie już istnieje!");
                return BadRequest(ModelState);
            }

            if (exerciseCreate == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(exerciseCreate);
            }

            var exercise = _mapper.Map<ExerciseModel>(exerciseCreate);
            await _data.InsertExercise(exercise);

            _response.Result = exercise;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Errors = new List<string> { ex.ToString() };
        }

        return _response;
    }

	[HttpPut]
	[Authorize(Roles = "admin")]
	[ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateExercise([FromBody] ExerciseUpdateDTO exerciseUpdate)
    {
        try
        {
            var exerciseId = await _data.GetExercise(exerciseUpdate.Id);
            if (exerciseId == null) 
            {
                return NotFound(); 
            }

            var exercise = _mapper.Map<ExerciseModel>(exerciseUpdate);
            await _data.UpdateExercise(exercise);

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

    [HttpDelete]
		[Authorize(Roles = "admin")]
		[HttpDelete("{id:int}", Name = "DeleteExercise")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> DeleteExercise(int id)
		{
			try
			{
				if (id == 0) 
            { 
                return BadRequest(); 
            }

				var exercise = await _data.GetExercise(id);

				if (exercise == null) 
            { 
                return NotFound(); 
            }

				await _data.DeleteExercise(id);
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
	}
