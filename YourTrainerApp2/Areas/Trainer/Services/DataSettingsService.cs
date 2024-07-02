using YourTrainer_App.Services.APIServices.IServices;
using YourTrainerApp.Models;
using static YourTrainer_Utility.StaticDetails;

namespace YourTrainer_App.Areas.Trainer.Services;


public class DataSettingsService : IDataSettingsService
{
	private readonly ITrainerDataService _trainerDataService;

	public DataSettingsService(ITrainerDataService trainerDataService)
	{
		_trainerDataService = trainerDataService;
	}

	public async Task<bool> TrainerDataIsPresent(int trainerId)
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(trainerId);
		return apiResponse.Result is not null;
	}
}
