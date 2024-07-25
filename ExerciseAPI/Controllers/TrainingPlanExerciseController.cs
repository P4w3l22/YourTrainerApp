using AutoMapper;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YourTrainer_DBDataAccess.Data.IData;

namespace ExerciseAPI.Controllers;

[Route("api/TrainingPlanExercise")]
[ApiController]
public class TrainingPlanExerciseController : Controller
{
	private readonly ITrainingPlanData _data;
	private readonly IMapper _mapper;
	protected APIResponse _response;

    public TrainingPlanExerciseController(ITrainingPlanData data, IMapper mapper)
    {
        _data = data;
		_mapper = mapper;
		_response = new();
    }

	[HttpGet("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> GetPlanExercises(int id)
	{
		try
		{
			if (await planIsNotPresent(id))
			{
				return NotFound();
			}

			var planExercises = await _data.GetPlanExercises(id);
			_response.Result = planExercises;
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

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> InsertPlanExercise([FromBody]TrainingPlanExerciseCreateDTO trainingPlanExerciseCreate)
	{
		try
		{
			if (await planIsNotPresent(trainingPlanExerciseCreate.TPId))
			{
				return NotFound();
			}

			var trainingPlanExercise = _mapper.Map<TrainingPlanExerciseModel>(trainingPlanExerciseCreate);
			await _data.InsertPlanExercise(trainingPlanExercise);

			_response.Result = trainingPlanExercise;
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
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> UpdatePlanExercise([FromBody] TrainingPlanExerciseUpdateDTO trainingPlanExerciseUpdate)
	{
		try
		{
			if (await planIsNotPresent(trainingPlanExerciseUpdate.TPId))
			{
				return NotFound();
			}

			var trainingPlanExercise = _mapper.Map<TrainingPlanExerciseModel>(trainingPlanExerciseUpdate);
			await _data.UpdatePlanExercise(trainingPlanExercise);

			_response.Result = trainingPlanExercise;
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

	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> DeletePlanExercise(int id)
	{
		try
		{
			if (await planIsNotPresent(id))
			{
				return NotFound();
			}

			await _data.DeletePlanExercise(id);

			_response.StatusCode = HttpStatusCode.NoContent;
			return Ok(_response);
		}
		catch (Exception ex)
		{
			_response.IsSuccess = false;
			_response.Errors = new List<string> { ex.ToString() };
		}
		return _response;
	}

	private async Task<bool> planIsNotPresent(int id) =>
		await _data.GetPlan(id) is null;
}
