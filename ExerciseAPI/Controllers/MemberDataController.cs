using AutoMapper;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YourTrainer_DBDataAccess.Data.IData;

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
			_response.StatusCode = HttpStatusCode.InternalServerError;
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
				_response.Errors = new List<string> { "Id ćwiczenia nie może być równe 0" };
				return BadRequest(_response);
			}

			MemberDataModel memberData = await _memberData.GetMember(id);

			if (memberData is null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.Errors = new List<string> { "Brak danego ćwiczenia" };
				_response.IsSuccess = false;
				return NotFound(_response);
			}

			_response.Result = memberData;
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}
		catch (Exception ex)
		{
			_response.StatusCode = HttpStatusCode.InternalServerError;
			_response.IsSuccess = false;
			_response.Errors = new List<string>() { ex.Message };
		}

		return _response;
	}

	[HttpPost]
	[Authorize(Roles = "gym member")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<APIResponse>> CreateMemberData([FromBody] MemberDataModel memberData)
	{
		try
		{
			MemberDataModel memberDataDb = await _memberData.GetMember(memberData.MemberId);

			if (memberDataDb is not null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.Errors = new List<string> { "Istnieją już dane dla tego użytkownika" };
				_response.IsSuccess = false;
				return BadRequest(ModelState);
			}

			await _memberData.InsertMemberData(memberData);

			_response.StatusCode = HttpStatusCode.Created;
			return Ok(_response);
		}
		catch (Exception ex)
		{
			_response.StatusCode = HttpStatusCode.InternalServerError;
			_response.IsSuccess = false;
			_response.Errors = new List<string> { ex.ToString() };
		}

		return _response;
	}

	[HttpPut]
	//[Authorize(Roles = "gym member")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> UpdateMemberData([FromBody] MemberDataModel memberData)
	{
		try
		{
			MemberDataModel memberDataDb = await _memberData.GetMember(memberData.MemberId);
			if (memberDataDb is null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.Errors = new List<string> { "Brak danego użytkownika" };
				_response.IsSuccess = false;
				return NotFound(_response);
			}

			await _memberData.UpdateMemberData(memberData);

			_response.StatusCode = HttpStatusCode.NoContent;
			_response.IsSuccess = true;
			return Ok(_response);
		}
		catch (Exception ex)
		{
			_response.StatusCode = HttpStatusCode.InternalServerError;
			_response.IsSuccess = false;
			_response.Errors = new List<string>() { ex.ToString() };
		}

		return _response;
	}

	[HttpDelete("{id:int}")]
	[Authorize(Roles = "gym member")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<APIResponse>> DeleteMemberData(int id)
	{
		try
		{
			if (id == 0)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.Errors = new List<string> { "Id użytkownika nie może być równe 0" };
				_response.IsSuccess = false;
				return BadRequest(_response);
			}

			var memberDataDb = await _memberData.GetMember(id);

			if (memberDataDb is null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.Errors = new List<string> { "Brak danego użytkownika" };
				_response.IsSuccess = false;
				return NotFound(_response);
			}

			await _memberData.DeleteMemberData(id);
			_response.StatusCode = HttpStatusCode.NoContent;
			_response.IsSuccess = true;
			return Ok(_response);
		}
		catch (Exception ex)
		{
			_response.StatusCode = HttpStatusCode.InternalServerError;
			_response.IsSuccess = false;
			_response.Errors = new List<string>() { ex.ToString() };
		}

		return _response;
	}
}
