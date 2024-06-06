using AutoMapper;
using DbDataAccess.Data;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExerciseAPI.Controllers;

[Route("api/MemberData")]
[ApiController]
public class MemberDataController : Controller
{
	private readonly IMapper _mapper;
	private readonly IMemberData _memberData;
	protected APIResponse _response;

    public MemberDataController(IMapper mapper, IMemberData memberData)
    {
		_mapper = mapper;
		_memberData = memberData;
		this._response = new();
    }

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<APIResponse>> GetMembers()
	{
		try
		{
			IEnumerable<MemberDataModel> membersList = await _memberData.GetMembers();
			_response.Result = membersList.ToList();
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
	public async Task<ActionResult<APIResponse>> GetMember(int id)
	{
		try
		{
			if (id == 0)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				return BadRequest(_response);
			}

			MemberDataModel memberData = await _memberData.GetMember(id);

			if (memberData is null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				return NotFound(_response);
			}

			_response.Result = memberData;
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
	public async Task<ActionResult<APIResponse>> CreateTrainerData([FromBody] MemberDataModel memberData)
	{
		try
		{
			var memberDataDb = await _memberData.GetMember(memberData.MemberId);

			if (memberDataDb is not null)
			{
				ModelState.AddModelError("Errors", "Istnieją już dane dla tego użytkownika");
				return BadRequest(ModelState);
			}

			if (memberData is null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				return BadRequest(memberData);
			}

			await _memberData.InsertMemberData(memberData);

			_response.Result = memberData;
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
	public async Task<ActionResult<APIResponse>> UpdateTrainerData([FromBody] MemberDataModel memberData)
	{
		try
		{
			var memberDataDb = await _memberData.GetMember(memberData.MemberId);
			if (memberDataDb is null)
			{
				return NotFound();
			}

			await _memberData.UpdateMemberData(memberData);

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

			var memberDataDb = await _memberData.GetMember(id);

			if (memberDataDb is null)
			{
				return NotFound();
			}

			await _memberData.DeleteMemberData(id);
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
