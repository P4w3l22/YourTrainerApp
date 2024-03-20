using ExerciseAPI.Models;
using YourTrainerApp2.Models;

namespace YourTrainerApp2.Services.IServices
{
	public interface IBaseService
	{
		APIResponse response { get; set; }
		Task<T> SendAsync<T>(APIRequest request);
	}
}
