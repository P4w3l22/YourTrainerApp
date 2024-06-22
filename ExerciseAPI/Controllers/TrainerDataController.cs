using AutoMapper;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YourTrainer_DBDataAccess.Data.IData;

namespace ExerciseAPI.Controllers;

[Route("api/TrainerData")]
[ApiController]
public class TrainerDataController : Controller
{
	private readonly IMapper _mapper;
	private readonly ITrainerData _trainerData;
	protected APIResponse _response;

    public TrainerDataController(IMapper mapper, ITrainerData trainerData)
    {
		_mapper = mapper;
		_trainerData = trainerData;
		this._response = new();
    }

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<APIResponse>> GetTrainers()
	{
		try
		{
			IEnumerable<TrainerDataModel> trainersList = await _trainerData.GetTrainers();
			_response.Result = trainersList.ToList();
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}
		catch (Exception ex)
		{
			_response.IsSuccess = false;
			_response.Errors = new List<string>() { ex.Message };
		}
		
		return _response;
	}

	[HttpGet("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> GetTrainer(int id)
	{
		try
		{
			if (id == 0)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				return BadRequest(_response);
			}

			TrainerDataModel trainerData = await _trainerData.GetTrainer(id);

			if (trainerData is null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				return NotFound(_response);
			}

			_response.Result = trainerData;
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}
		catch (Exception ex)
		{
			_response.IsSuccess = false;
			_response.Errors = new List<string>() { ex.Message };
		}

		return _response;
	}

	[HttpPost]
	//[Authorize(Roles = "trainer")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<APIResponse>> CreateTrainerData([FromBody] TrainerDataModel trainerData)
	{
		try
		{
			var trainerDataDb = await _trainerData.GetTrainer(trainerData.TrainerId);

			if (trainerDataDb is not null)
			{
				ModelState.AddModelError("Errors", "Istnieją już dane dla tego trenera");
				return BadRequest(ModelState);
			}

			if (trainerData is null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				return BadRequest(trainerData);
			}

			await _trainerData.InsertTrainerData(trainerData);

			_response.Result = trainerData;
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
	//[Authorize(Roles = "trainer")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> UpdateTrainerData([FromBody] TrainerDataModel trainerData)
	{
		try
		{
			var trainerDataDb = await _trainerData.GetTrainer(trainerData.TrainerId);
			if (trainerDataDb is null)
			{
				return NotFound();
			}

			await _trainerData.UpdateTrainerData(trainerData);

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

	[HttpDelete("{id:int}")]
	//[Authorize(Roles = "trainer")]
	//[HttpDelete("{id:int}", Name = "DeleteExercise")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> DeleteTrainerData(int id)
	{
		try
		{
			if (id == 0)
			{
				return BadRequest();
			}

			var trainerDataDb = await _trainerData.GetTrainer(id);

			if (trainerDataDb is null)
			{
				return NotFound();
			}

			await _trainerData.DeleteTrainerData(id);
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
