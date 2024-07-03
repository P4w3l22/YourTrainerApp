using Newtonsoft.Json;
using YourTrainer_App.Services.APIServices;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainerApp.Models;
using static YourTrainer_Utility.StaticDetails;

namespace YourTrainer_App.Areas.Trainer.Services;


public class TrainerDataSettingsService : ITrainerDataSettingsService
{
	private readonly ITrainerDataService _trainerDataService;

	public TrainerDataSettingsService(ITrainerDataService trainerDataService)
	{
		_trainerDataService = trainerDataService;
	}

	public async Task UpdateTrainerData(TrainerDataModel trainerData) =>
		await _trainerDataService.UpdateAsync<APIResponse>(trainerData);

	public async Task CreateTrainerData(TrainerDataModel trainerData) =>
		await _trainerDataService.CreateAsync<APIResponse>(trainerData);

	public async Task ClearTrainerData(int trainerId) =>
		await _trainerDataService.DeleteAsync<APIResponse>(trainerId);

	public async Task<bool> TrainerDataIsPresent(int trainerId)
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(trainerId);
		return apiResponse.Result is not null;
	}

	public async Task<TrainerDataModel> GetTrainerDataFromDb(int trainerId)
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(trainerId);
		return JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));
	}

	public TrainerDataModel GetTrainerDataDefault(int trainerId, string username) =>
	new()
	{
		TrainerId = trainerId,
		Email = username
	};
}
