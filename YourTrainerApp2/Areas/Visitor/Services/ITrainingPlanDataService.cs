using YourTrainerApp.Models;
using YourTrainerApp.Models.VM;

namespace YourTrainer_App.Areas.Visitor.Services
{
	public interface ITrainingPlanDataService
	{
		Task<List<TrainingPlanExerciseCreateVM>> AddExerciseAndGetExercisesList(List<TrainingPlanExerciseCreateVM> exercises, int id);
		Task<TrainingPlan> AddExerciseAndGetTrainingPlan(TrainingPlan trainingPlan, int id);
		List<int> GetPreviousTrainingPlanExercises(List<TrainingPlanExercise> trainingPlanExercises);
		Task CreateTrainingPlan(TrainingPlan trainingPlan);
		Task<int> GetTrainingPlanId(string title, string creator);
		Task SetTrainingPlanToClient(int trainerId, int clientId, int planId, string token);
		TrainingPlan DecrementExerciseSeriesAndGetTrainingPlan(TrainingPlan trainingPlan, int id);
		Task<TrainingPlan> DeleteExerciseAndGetTrainingPlan(TrainingPlan trainingPlan, int listPosition);
		List<TrainingPlanExerciseCreateVM> DeleteExerciseAndGetExercisesList(List<TrainingPlanExerciseCreateVM> exercises, int listPosition);
		Task DeleteTrainingPlan(int id);
		Task<TrainingPlan> GetTrainingPlan(int id);
		Task<List<TrainingPlanExerciseCreateVM>> GetTrainingPlanExercises(List<TrainingPlanExercise> trainingPlanExercises);
		Task<List<TrainingPlan>> GetUserTrainingPlans(string sessionUsername);
		TrainingPlan IncrementExerciseSeriesAndGetTrainingPlan(TrainingPlan trainingPlan, int id);
		TrainingPlan SaveNotesAndGetTrainingPlan(TrainingPlan trainingPlan, string notes);
		TrainingPlan SaveRepsWeightsAndGetTrainingPlan(TrainingPlan trainingPlan, string values, string exerciseId, string seriesPosition);
		TrainingPlan SaveTitleAndGetTrainingPlan(TrainingPlan trainingPlan, string title);
		TrainingPlan SaveTrainingDaysAndGetTrainingPlan(TrainingPlan trainingPlan, string day);
		Task UpdateTrainingPlan(TrainingPlan trainingPlan, List<int> previousExercusesId);
	}
}