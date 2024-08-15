using YourTrainer_App.Models;

namespace YourTrainer_App.Services.APIServices.IServices;

public interface IBaseService
{
    APIResponse response { get; set; }
    Task<T> SendAsync<T>(APIRequest request);
}
