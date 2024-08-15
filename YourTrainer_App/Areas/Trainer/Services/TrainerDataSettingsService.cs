using Newtonsoft.Json;
using YourTrainer_App.Services.APIServices;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_App.Services.DataServices;
using YourTrainer_App.Models;
using static YourTrainer_Utility.StaticDetails;

namespace YourTrainer_App.Areas.Trainer.Services;


public class TrainerDataSettingsService : ITrainerDataSettingsService
{
	private readonly ITrainerDataService _trainerDataService;
	private readonly ICooperationProposalService _cooperationProposalService;

	public TrainerDataSettingsService(ITrainerDataService trainerDataService, ICooperationProposalService cooperationProposalService)
	{
		_trainerDataService = trainerDataService;
		_cooperationProposalService = cooperationProposalService;
	}

	public async Task UpdateTrainerData(TrainerDataModel trainerData) =>
		await _trainerDataService.UpdateAsync<APIResponse>(trainerData);

	public async Task CreateTrainerData(TrainerDataModel trainerData) =>
		await _trainerDataService.CreateAsync<APIResponse>(trainerData);

	public async Task ClearTrainerData(int trainerId)
	{
		TrainerDataModel trainerData = await GetTrainerDataFromDb(trainerId);
		string[] trainerClientsId = trainerData.MembersId.Split(";");
		foreach (string clientId in trainerClientsId)
		{
			await _cooperationProposalService.DeleteTrainerClientCooperation(int.Parse(clientId));
		}

		await _trainerDataService.DeleteAsync<APIResponse>(trainerId);

	}

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
