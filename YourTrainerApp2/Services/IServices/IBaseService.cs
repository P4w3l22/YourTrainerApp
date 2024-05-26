using YourTrainerApp.Models;

namespace YourTrainerApp.Services.IServices;

public interface IBaseService
{
	APIResponse response { get; set; }
	Task<T> SendAsync<T>(APIRequest request);
}
