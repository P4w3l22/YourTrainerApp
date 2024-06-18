using DbDataAccess.Models;

namespace DbDataAccess.Data;

public interface ITrainerData
{
	Task DeleteTrainerData(int id);
	Task<TrainerDataModel> GetTrainer(int id);
	Task<IEnumerable<TrainerDataModel>> GetTrainers();
	Task InsertTrainerData(TrainerDataModel trainerData);
	Task UpdateTrainerData(TrainerDataModel trainerData);
}