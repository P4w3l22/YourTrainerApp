using YourTrainer_App.Areas.Auth.Models;

namespace YourTrainer_App.Services.APIServices.IServices;

public interface IAuthService
{
    public Task<T> LoginAsync<T>(LoginRequest loginRequest);
    public Task<T> RegisterAsync<T>(RegisterationRequest userData);
}