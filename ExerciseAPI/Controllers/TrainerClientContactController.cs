using AutoMapper;
using DbDataAccess.Data;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExerciseAPI.Controllers;

[Route("api/TrainerClientContact")]
[ApiController]
public class TrainerClientContactController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITrainerClientContactData _data;
    protected APIResponse _response;
    public TrainerClientContactController(IMapper mapper, ITrainerClientContactData data)
    {
        _mapper = mapper;
        _data = data;
        this._response = new();
    }

    [HttpGet("{SenderId:int}/{ReceiverId:int}/{MessageType}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetMessages(int SenderId, int ReceiverId, string MessageType)
    {
        try
        {
            IEnumerable<TrainerClientContact> messages = await _data.GetMessages(SenderId, ReceiverId, MessageType);
            _response.Result = _mapper.Map<List<TrainerClientContact>>(messages);
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> SendMessage([FromBody]TrainerClientContact trainerClientContactSend)
    {
		try
		{
			if (trainerClientContactSend is null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				return BadRequest(trainerClientContactSend);
			}

			
			await _data.SendMessage(trainerClientContactSend);

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

	[HttpPut("{id:int}")]
	//[Authorize(Roles = "admin")]
	[ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> SetAsRead(int id)
    {
        try
        {
            if (id < 1)
            {
                _response.StatusCode=HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

			await _data.SetAsRead(id);

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
