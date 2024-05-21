using YourTrainerApp.Areas.Auth.Models;

namespace YourTrainerApp.Services.IServices;

public interface IAuthService
{
    public Task<T> LoginAsync<T>(LoginRequest loginRequest);
    public Task<T> RegisterAsync<T>(RegisterationRequest userData);
}