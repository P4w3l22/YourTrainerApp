using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainerApp.Models;

namespace YourTrainer_App.Services.APIServices;

public class AssignedTrainingPlanService : BaseService, IAssignedTrainingPlanService
{
	private string? APIUrl;
	public AssignedTrainingPlanService(IHttpClientFactory client, IConfiguration configuration) : base(client)
	{
		APIUrl = configuration.GetValue<string>("ServiceUrls:ExerciseAPI");
	}

	public Task<T> GetAllAsync<T>(int trainerId) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.GET,
			Url = APIUrl + "/api/AssignedTrainingPlan/trainer/" + trainerId
		});

	public Task<T> GetAsync<T>(int clientId) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.GET,
			Url = APIUrl + "/api/AssignedTrainingPlan/client/" + clientId
		});

	public Task<T> CreateAsync<T>(AssignedTrainingPlan assignedTrainingPlan, string token) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.POST,
			Url = APIUrl + "/api/AssignedTrainingPlan/",
			Data = assignedTrainingPlan,
			Token = token
		});

	public Task<T> DeleteAsync<T>(int id, string token) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.DELETE,
			Url = APIUrl + "/api/AssignedTrainingPlan/" + id,
			Token = token
		});

}
