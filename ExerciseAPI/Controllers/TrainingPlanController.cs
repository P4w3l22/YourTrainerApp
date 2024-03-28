using AutoMapper;
using DbDataAccess.Data;
using ExerciseAPI.Models;
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
	}
}
