using AutoMapper;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YourTrainer_DBDataAccess.Data.IData;
using YourTrainer_DBDataAccess.Models;

namespace YourTrainer_API.Controllers;

[Route("api/AssignedTrainingPlan")]
[ApiController]
public class AssignedTrainingPlanController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IAssignedTrainingPlanData _data;
	private readonly ITrainerData _trainerData;
	private readonly IMemberData _memberData;
	//private readonly ITrainingPlanData _trainingPlanData;
	protected APIResponse _response;

    public AssignedTrainingPlanController(IMapper mapper, IAssignedTrainingPlanData data, ITrainerData trainerData, IMemberData memberData/*, ITrainingPlanData trainingPlanData*/)
    {
        _mapper = mapper;
		_data = data;
		_trainerData = trainerData;
		_memberData = memberData;
		//_trainingPlanData = trainingPlanData;
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
	[Authorize(Roles = "trainer")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<APIResponse>> CreateAssignedPlan([FromBody] AssignedTrainingPlan assignedPlanCreate)
	{
		try
		{
			// Dodać sprawdzanie czy trener, plan i klient istnieją w bazie danych
			TrainerDataModel trainerData = await _trainerData.GetTrainer(assignedPlanCreate.TrainerId);
			MemberDataModel memberData = await _memberData.GetMember(assignedPlanCreate.ClientId);
			//TrainingPlanModel trainingPlan = await _trainingPlanData.GetPlan(assignedPlanCreate.PlanId);

			if (trainerData is null)
			{
				ModelState.AddModelError("Errors", "Nie ma trenera o takim id");
				return BadRequest(assignedPlanCreate);
			}
			else if (memberData is null)
			{
				ModelState.AddModelError("Errors", "Nie ma użytkownika o takim id");
				return BadRequest(assignedPlanCreate);
			}
			//else if (trainingPlan is null)
			//{
			//	ModelState.AddModelError("Errors", "Nie ma planu o takim id");
			//	return BadRequest(assignedPlanCreate);
			//}

			if (assignedPlanCreate is null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				return BadRequest(assignedPlanCreate);
			}

			var assignedPlan = assignedPlanCreate;
			await _data.InsertAssignedPlan(assignedPlan);

			_response.StatusCode = HttpStatusCode.Created;
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
	[Authorize(Roles = "trainer")]
	[ProducesResponseType(StatusCodes.Status200OK)]
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

			AssignedTrainingPlan assignedTrainingPlan = await _data.GetAssignedPlan(id);
			if (assignedTrainingPlan is null)
			{
				return NotFound();
			}

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
