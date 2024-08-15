using AutoMapper;
using YourTrainer_DBDataAccess.Models;
using YourTrainer_API.Models;
using YourTrainer_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YourTrainer_DBDataAccess.Data.IData;

namespace YourTrainer_API.Controllers;

[Route("api/YourTrainer_API")]
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
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<APIResponse>> GetExercises()
    {
        try
        {
            IEnumerable<ExerciseModel> exerciseList = await _data.GetExercises();
            _response.Result = _mapper.Map<List<ExerciseDTO>>(exerciseList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
			_response.StatusCode = HttpStatusCode.InternalServerError;
			_response.IsSuccess = false;
            _response.Errors = new List<string> { ex.Message };
        }
        return _response;
    }

    [HttpGet("{id:int}", Name = "GetExercise")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<APIResponse>> GetExercise(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
				_response.Errors = new List<string> { "Id ćwiczenia nie może być równe 0" };
				_response.IsSuccess = false;
				return BadRequest(_response);
            }

            ExerciseModel exercise = await _data.GetExercise(id);

            if (exercise is null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
				_response.Errors = new List<string> { "Brak danego ćwiczenia" };
				_response.IsSuccess = false;
				return NotFound(_response);
            }
            _response.Result = _mapper.Map<ExerciseDTO>(exercise);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
			_response.StatusCode = HttpStatusCode.InternalServerError;
			_response.IsSuccess = false;
            _response.Errors = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpPost]
	[Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateExercise([FromBody]ExerciseCreateDTO exerciseCreate)
    {
        try
        {
            IEnumerable<ExerciseModel> exercises = await _data.GetExercises();
			List<ExerciseModel> exerciseList = exercises.Where(e => e?.Name?.ToLower() == exerciseCreate?.Name?.ToLower()).ToList();
            if (exerciseList.Count > 0)
            {
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.Errors = new List<string> { "Ćwiczenie o tej nazwie już istnieje" };
				_response.IsSuccess = false;
                return BadRequest(_response);
            }

            var exercise = _mapper.Map<ExerciseModel>(exerciseCreate);
            await _data.InsertExercise(exercise);

            _response.StatusCode = HttpStatusCode.Created;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.IsSuccess = false;
            _response.Errors = new List<string> { ex.ToString() };
        }

        return _response;
    }

	[HttpPut]
	[Authorize(Roles = "admin")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<APIResponse>> UpdateExercise([FromBody] ExerciseUpdateDTO exerciseUpdate)
    {
        try
        {
            ExerciseModel exerciseId = await _data.GetExercise(exerciseUpdate.Id);
            
            if (exerciseId is null) 
            {
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.Errors = new List<string> { "Brak danego ćwiczenia" };
				_response.IsSuccess = false;
				return NotFound(_response);
			}

            var exercise = _mapper.Map<ExerciseModel>(exerciseUpdate);
            await _data.UpdateExercise(exercise);

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
			_response.StatusCode = HttpStatusCode.InternalServerError;
			_response.IsSuccess = false;
			_response.Errors = new List<string> { ex.ToString() };
		}

        return _response;
    }

	[Authorize(Roles = "admin")]
	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<APIResponse>> DeleteExercise(int id)
	{
		try
		{
			if (id == 0) 
            {
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.Errors = new List<string> { "Id ćwiczenia nie może być równe 0" };
				_response.IsSuccess = false;
				return BadRequest(); 
            }

			var exercise = await _data.GetExercise(id);

			if (exercise is null) 
            {
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.Errors = new List<string> { "Brak danego ćwiczenia" };
				_response.IsSuccess = false;
				return NotFound(_response);
			}

			await _data.DeleteExercise(id);
			_response.StatusCode = HttpStatusCode.NoContent;
			_response.IsSuccess = true;

			return Ok(_response);
		}
		catch (Exception ex)
		{
			_response.StatusCode = HttpStatusCode.InternalServerError;
			_response.IsSuccess = false;
			_response.Errors = new List<string>() { ex.ToString() };
		}

		return _response;
	}
}
