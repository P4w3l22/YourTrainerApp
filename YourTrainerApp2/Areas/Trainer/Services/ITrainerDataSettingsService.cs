using YourTrainerApp.Models;

namespace YourTrainer_App.Areas.Trainer.Services;

public interface ITrainerDataSettingsService
{
	Task ClearTrainerData(int trainerId);
	Task CreateTrainerData(TrainerDataModel trainerData);
	TrainerDataModel GetTrainerDataDefault(int trainerId, string username);
	Task<TrainerDataModel> GetTrainerDataFromDb(int trainerId);
	Task<bool> TrainerDataIsPresent(int trainerId);
	Task UpdateTrainerData(TrainerDataModel trainerData);
}