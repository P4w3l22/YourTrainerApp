using DbDataAccess.Models;

namespace DbDataAccess.Data;

public interface ITrainingPlanData
{
	Task DeletePlan(int id);
	Task DeletePlanExercise(int id);
	Task<IEnumerable<TrainingPlanModel>> GetAllPlans();
	Task<TrainingPlanModel> GetPlan(int id);
	Task<IEnumerable<TrainingPlanExerciseModel>> GetPlanExercises(int id);
	Task InsertPlan(TrainingPlanModel model);
	Task InsertPlanExercise(TrainingPlanExerciseModel model);
	Task UpdatePlan(TrainingPlanModel model);
	Task UpdatePlanExercise(TrainingPlanExerciseModel model);
}