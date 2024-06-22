using AutoMapper;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YourTrainer_DBDataAccess.Data.IData;

namespace ExerciseAPI.Controllers;

[Route("api/TrainerClientContact")]
[ApiController]
public class TrainerClientContactController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITrainerClientContactData _data;
	//private readonly ITrainerData _trainerData;
	//private readonly IMemberData _memberData;
	protected APIResponse _response;
    public TrainerClientContactController(IMapper mapper, ITrainerClientContactData data/*, ITrainerData trainerData, IMemberData memberData*/)
    {
        _mapper = mapper;
        _data = data;
        //_trainerData = trainerData;
        //_memberData = memberData;
        this._response = new();
    }

    [HttpGet("{SenderId:int}/{ReceiverId:int}/{MessageType}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetMessages(int senderId, int receiverId, string messageType)
    {
        try
        {
			//TrainerDataModel senderAsTrainerData = await _trainerData.GetTrainer(senderId);
			//MemberDataModel receiverAsMemberData = await _memberData.GetMember(receiverId);

			//if (senderAsTrainerData is null)
			//{
			//	MemberDataModel senderAsMemberData = await _memberData.GetMember(receiverId);
			//	if (senderAsMemberData is null)
			//	{
			//		ModelState.AddModelError("Errors", "Nie ma wysyłającego o takim id");
			//		return BadRequest();
			//	}
			//}
			//if (receiverAsMemberData is null)
			//{
			//	TrainerDataModel receiverAsTrainerData = await _trainerData.GetTrainer(senderId);
			//	if (receiverAsTrainerData is null)
			//	{
			//		ModelState.AddModelError("Errors", "Nie ma odbiorcy o takim id");
			//		return BadRequest();
			//	}
			//}

			IEnumerable<TrainerClientContact> messages = await _data.GetMessages(senderId, receiverId, messageType);
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

	[HttpGet("{receiverId:int}/{messageType}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<APIResponse>> GetCooperationProposals(int receiverId, string messageType)
	{
		try
		{
			IEnumerable<TrainerClientContact> messages = await _data.GetCooperationProposals(receiverId, messageType);
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
