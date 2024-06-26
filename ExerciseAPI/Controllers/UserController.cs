using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YourTrainer_DBDataAccess.Data.IData;
using LoginRequest = DbDataAccess.Models.LoginRequest;

namespace ExerciseAPI.Controllers;

[Route("/api/UserAuth")]
[ApiController]
public class UserController : Controller
{
	private readonly ILocalUserData _data;
    private readonly APIResponse _response;
	private string? _token;

    public UserController(ILocalUserData data, IConfiguration configuration)
    {
        _data = data;
        _response = new();
        _token = configuration.GetValue<string>("ApiSettings:SecretToken");
    }

    [HttpPost("Login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var loginResponse = await _data.Login(loginRequest, _token);
        if (loginResponse.User is null || loginResponse.Token == string.Empty)
        {
            if (loginResponse.Errors is not null)
            {
                _response.IsSuccess = false;
                _response.Errors = loginResponse.Errors;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }

        _response.IsSuccess = true;
        _response.Result = loginResponse;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
	}

	[HttpPost("Register")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register([FromBody] RegisterationRequest registerRequest)
    {
        if (registerRequest.UserName is not null)
        {
            if (!await _data.IsUniqueUser(registerRequest.UserName))
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { "Podana nazwa użytkownika już jest zajęta" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }
        else
        {
			_response.IsSuccess = false;
			_response.Errors = new List<string>() { "Nazwa użytkownika nie może być pusta" };
			return BadRequest(_response);
        }
        
        var registerUser = await _data.Register(registerRequest);

        if (registerUser is null)
        {
            _response.IsSuccess = false;
            _response.Errors = new List<string>() { "Wystąpił błąd podczas rejestracji" };
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        var loginResponse = await _data.Login(new LoginRequest()
        {
            UserName = registerRequest.UserName,
            Password = registerRequest.Password
        }, _token);

        _response.IsSuccess = true;
        _response.Result = loginResponse;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }
}
