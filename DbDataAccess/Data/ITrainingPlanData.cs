using DbDataAccess.Models;

namespace DbDataAccess.Data
{
	public interface ITrainingPlanData
	{
		Task DeletePlan(int id);
		Task DeletePlanExercise(int id);
		Task<IEnumerable<TrainingPlanModel>> GetAllPlans();
		Task<TrainingPlanWithExercisesModel> GetPlan(int id);
		Task<TrainingPlanExerciseModel> GetPlanExercise(int id);
		Task InsertPlan(TrainingPlanModel model);
		Task InsertPlanExercise(TrainingPlanExerciseModel model);
		Task UpdatePlan(TrainingPlanModel model);
		Task UpdatePlanExercise(TrainingPlanExerciseModel model);
	}
}