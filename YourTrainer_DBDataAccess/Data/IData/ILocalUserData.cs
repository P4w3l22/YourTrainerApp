using YourTrainer_DBDataAccess.Models;

namespace YourTrainer_DBDataAccess.Data.IData;

public interface ILocalUserData
{
    Task<LoginResponse> Login(LoginRequest loginRequest, string token);
    Task<LocalUserModel> Register(RegisterationRequest registerationRequest);
    Task<bool> IsUniqueUser(string userName);
}