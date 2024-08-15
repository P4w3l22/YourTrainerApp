﻿using AutoMapper;
using YourTrainer_DBDataAccess.Models;
using YourTrainer_API.Models;
using YourTrainer_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YourTrainer_DBDataAccess.Data.IData;

namespace YourTrainer_API.Controllers;

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
			var plan = await _data.GetPlan(id);
			if (plan == null)
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
			var plan = await _data.GetPlan(trainingPlanExerciseCreate.TPId);
			if (plan == null)
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
			var plan = await _data.GetPlan(trainingPlanExerciseUpdate.TPId);
			if (plan == null)
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
			var plan = await _data.GetPlan(id);
			if (plan == null)
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
}
