using AutoMapper;
using DbDataAccess.Data;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YourTrainer_DBDataAccess.Models;

namespace YourTrainer_API.Controllers;

[Route("api/AssignedTrainingPlan")]
[ApiController]
public class AssignedTrainingPlanController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IAssignedTrainingPlanData _data;
    protected APIResponse _response;


    public AssignedTrainingPlanController(IMapper mapper, IAssignedTrainingPlanData data)
    {
        _mapper = mapper;
		_data = data;
		this._response = new();
	}

	[HttpGet("trainer/{trainerId:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<APIResponse>> GetAssignedTrainingPlans(int trainerId)
	{
		try
		{
			IEnumerable<AssignedTrainingPlan> exerList = await _data.GetAssignedPlans(trainerId);
			_response.Result = _mapper.Map<List<AssignedTrainingPlan>>(exerList);
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
	[HttpGet("client/{clientId:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> GetAssignedPlan(int clientId)
	{
		try
		{
			if (clientId == 0)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				return BadRequest(_response);
			}

			var assignedPlan = await _data.GetAssignedPlan(clientId);

			if (assignedPlan is null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				return NotFound();
			}
			_response.Result = assignedPlan;
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
	//[Authorize(Roles = "admin")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<APIResponse>> CreateAssignedPlan([FromBody] AssignedTrainingPlan assignedPlanCreate)
	{
		try
		{
			// Dodać sprawdzanie czy trener, plan i klient istnieją w bazie danych
			//var exercises = await _data.GetAssignedPlan();
			//var exList = exercises.Where(e => e.Name.ToLower() == exerciseCreate.Name.ToLower()).ToList();
			//if (exList.Count > 0)
			//{
			//	ModelState.AddModelError("Errors", "Ćwiczenie już istnieje!");
			//	return BadRequest(ModelState);
			//}

			if (assignedPlanCreate == null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				return BadRequest(assignedPlanCreate);
			}

			var assignedPlan = assignedPlanCreate;
			await _data.InsertAssignedPlan(assignedPlan);

			_response.Result = assignedPlan;
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


	//[Authorize(Roles = "admin")]
	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> DeleteAssignedPlan(int id)
	{
		try
		{
			if (id == 0)
			{
				return BadRequest();
			}

			//var exercise = await _data.GetExercise(id);

			//if (exercise == null)
			//{
			//	return NotFound();
			//}

			await _data.DeleteAssignedPlan(id);
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
