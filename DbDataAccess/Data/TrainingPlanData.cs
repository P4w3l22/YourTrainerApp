using DbDataAccess.DbAccess;
using DbDataAccess.Models;
using YourTrainer_DBDataAccess.Data.IData;

namespace DbDataAccess.Data;

public class TrainingPlanData : ITrainingPlanData
{
	private readonly ISqlDataAccess _db;

	public TrainingPlanData(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task<IEnumerable<TrainingPlanModel>> GetAllPlans()
	{
		IEnumerable<TrainingPlanModel> plans = await _db.GetData<TrainingPlanModel, dynamic>("dbo.spTrainingPlan_GetAll", new { });

		foreach (var plan in plans)
		{
			plan.Exercises = await SetPlanExercises(plan.Id);
		}
		return plans;
	}

	public async Task<TrainingPlanModel> GetPlan(int id)
	{
		var plans = await _db.GetData<TrainingPlanModel, dynamic>("spTrainingPlan_Get", new { Id = id });
		var plan = plans.FirstOrDefault() ?? throw new InvalidOperationException("Brak planu o tym id w bazie danych"); ;
		plan.Exercises = await SetPlanExercises(id);
		return plan;
	}

	private async Task<List<TrainingPlanExerciseModel>> SetPlanExercises(int id)
	{
		IEnumerable<TrainingPlanExerciseModel> planExercisesFromDb = await _db.GetData<TrainingPlanExerciseModel, dynamic>("spTrainingPlanExercises_GetAll", new { TPId = id });
		return planExercisesFromDb.ToList();
	}
	

	public async Task InsertPlan(TrainingPlanModel model) =>
		await _db.SaveData("spTrainingPlan_Insert", new
		{
			model.Title,
			model.TrainingDays,
			model.Notes,
			model.Creator
		});

	public async Task UpdatePlan(TrainingPlanModel model) =>
		await _db.SaveData("spTrainingPlan_Update", new
		{
			model.Id,
			model.Title,
			model.TrainingDays,
			model.Notes,
			model.Creator
		});

	public async Task DeletePlan(int id) =>
		await _db.SaveData("spTrainingPlan_Delete", new { Id = id });


	public async Task<IEnumerable<TrainingPlanExerciseModel>> GetPlanExercises(int id) =>
		await _db.GetData<TrainingPlanExerciseModel, dynamic>("spTrainingPlanExercises_GetAll", new { TPId = id });

	public async Task InsertPlanExercise(TrainingPlanExerciseModel model) =>
		await _db.SaveData("spTrainingPlanExercise_Insert", new
		{
			model.TPId,
			model.EId,
			model.Series,
			model.Reps,
			model.Weights
		});

	public async Task UpdatePlanExercise(TrainingPlanExerciseModel model) =>
		await _db.SaveData("spTrainingPlanExercise_Update", model);

	public async Task DeletePlanExercise(int id) =>
		await _db.SaveData("spTrainingPlanExercise_Delete", new { Id = id });
}
