using DbDataAccess.Models;

namespace DbDataAccess.Data;

public interface ILocalUserData
{
	Task<LoginResponse> Login(LoginRequest loginRequest, string token);
	Task<LocalUserModel> Register(RegisterationRequest registerationRequest);
}