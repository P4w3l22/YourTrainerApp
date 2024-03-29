using AutoMapper;
using DbDataAccess.Data;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExerciseAPI.Controllers
{
	[Route("api/TrainingPlan")]
	[ApiController]
	public class TrainingPlanController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ITrainingPlanData _data;
		protected APIResponse _response;

        public TrainingPlanController(IMapper mapper, ITrainingPlanData data)
        {
			_mapper = mapper;
			_data = data;
			_response = new();            
        }

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetAllPlans()
		{
			try
			{
				var plans = await _data.GetAllPlans();
				_response.Result = plans;
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

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetPlan(int id)
		{
			try
			{
				var plan = await _data.GetPlan(id);
				_response.Result = plan;
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> InsertPlan([FromBody]TrainingPlanCreateDTO trainingPlanCreate)
		{
			try
			{
				if (trainingPlanCreate == null)
				{
					return BadRequest();
				}

				var trainingPlan = _mapper.Map<TrainingPlanModel>(trainingPlanCreate);

				await _data.InsertPlan(trainingPlan);

				_response.StatusCode = HttpStatusCode.NoContent;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Errors = new List<string>() { ex.ToString() };
			}
			return _response;
		}


		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> UpdatePlan([FromBody] TrainingPlanUpdateDTO trainingPlanUpdate)
		{
			try
			{
				if (trainingPlanUpdate == null)
				{
					return BadRequest();
				}

				var trainingPlan = _mapper.Map<TrainingPlanModel>(trainingPlanUpdate);

				await _data.UpdatePlan(trainingPlan);

				_response.StatusCode = HttpStatusCode.NoContent;
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> DeletePlan(int id)
		{
			try
			{
				var plan = await _data.GetPlan(id);
				if (plan == null)
				{
					return NotFound();
				}

				await _data.DeletePlan(id);

				_response.StatusCode = HttpStatusCode.NoContent;
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
}
