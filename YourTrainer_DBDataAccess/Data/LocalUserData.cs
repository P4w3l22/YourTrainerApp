using YourTrainer_DBDataAccess.DbAccess;
using YourTrainer_DBDataAccess.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YourTrainer_DBDataAccess.Data.IData;

namespace YourTrainer_DBDataAccess.Data;

public class LocalUserData : ILocalUserData
{
	private readonly ISqlDataAccess _db;
	private string _token;

	public LocalUserData(ISqlDataAccess db, IConfiguration configuration)
	{
		_db = db;
	}


	public async Task<LoginResponse> Login(LoginRequest loginRequest, string token)
	{
		_token = token;
		LocalUserModel user;
		try
		{
			user = await GetUser(loginRequest);

		}
		catch (ArgumentNullException)
		{
			return new LoginResponse()
			{
				User = null,
				Token = string.Empty,
				Errors = new List<string>() { "Niewłaściwy login" }
			};
		}
		catch (ArgumentException)
		{
			return new LoginResponse()
			{
				User = null,
				Token = string.Empty,
				Errors = new List<string>() { "Błędne hasło" }
			};
		}

		LoginResponse loginResponse = new()
		{
			User = user,
			Token = GenerateTokenString(user)
		};

		return loginResponse;
	}


	private async Task<LocalUserModel> GetUser(LoginRequest loginRequest)
	{
		var user = await GetUserAccountByUserName(loginRequest.UserName);
		if (user is null)
		{
			throw new ArgumentNullException();
		}

		if (user.Password != loginRequest.Password)
		{
			throw new ArgumentException();
		}

		return user;
	}


	private async Task<LocalUserModel> GetUserAccountByUserName(string userName)
	{
		var users = await _db.GetData<LocalUserModel, dynamic>("spLocalUsers_Get", new { userName });
		return users.FirstOrDefault();
	}

	
	private string GenerateTokenString(LocalUserModel user)
	{
		JwtSecurityTokenHandler tokenHandler = new();
		var key = Encoding.ASCII.GetBytes(_token);

		SecurityTokenDescriptor tokenDescriptor = new()
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, user.Id.ToString()),
				new Claim(ClaimTypes.Role, user.Role)
			}),
			Expires = DateTime.UtcNow.AddDays(7),
			SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}


	public async Task<LocalUserModel> Register(RegisterationRequest registerationRequest)
	{
		LocalUserModel user = new()
		{
			UserName = registerationRequest.UserName,
			Name = registerationRequest.Name,
			Password = registerationRequest.Password,
			Role = registerationRequest.Role
		};

		await _db.SaveData("spLocalUsers_Insert", new
		{
			UserName = user.UserName,
			Name = user.Name,
			Password = user.Password,
			Role = user.Role
		});

		user.Password = string.Empty;
		return user;
	}


	public async Task<bool> IsUniqueUser(string userName) =>
		await GetUserAccountByUserName(userName) is null;
}
