using YourTrainer_Utility;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;

namespace YourTrainerApp.Services;

public class ExerciseService : BaseService, IExerciseService
{
	private string APIUrl;
    public ExerciseService(IHttpClientFactory client, IConfiguration configuration) : base(client)
    {
	APIUrl = configuration.GetValue<string>("ServiceUrls:ExerciseAPI");
    }

	public Task<T> GetAllAsync<T>() =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.GET,
			Url = APIUrl + "/api/ExerciseAPI/"
		});

	public Task<T> GetAsync<T>(int id) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.GET,
			Url = APIUrl + "/api/ExerciseAPI/" + id
		});

	public Task<T> CreateAsync<T>(Exercise exercise, string token) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.POST,
			Url = APIUrl + "/api/ExerciseAPI/",
			Data = exercise,
			Token = token
		});

	public Task<T> UpdateAsync<T>(Exercise exercise, string token) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.PUT,
			Url = APIUrl + "/api/ExerciseAPI/",
			Data = exercise,
			Token = token
		});

	public Task<T> DeleteAsync<T>(int id, string token) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.DELETE,
			Url = APIUrl + "/api/ExerciseAPI/" + id,
			Token = token
		});

}
