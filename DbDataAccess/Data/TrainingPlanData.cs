
using DbDataAccess.DbAccess;
using DbDataAccess.Models;

namespace DbDataAccess.Data;

public class TrainingPlanData : ITrainingPlanData
{
	private readonly ISqlDataAccess _db;

	public TrainingPlanData(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task<IEnumerable<TrainingPlanModel>> GetAllPlans() =>
		await _db.GetData<TrainingPlanModel, dynamic>("dbo.spTrainingPlan_GetAll", new { });

	public async Task<TrainingPlanWithExercisesModel> GetPlan(int id)
	{
		var plans = await _db.GetData<TrainingPlanModel, dynamic>("spTrainingPlan_Get", new { Id = id });
		var plan = plans.FirstOrDefault();

		var exercises = await _db.GetData<TrainingPlanExerciseModel, dynamic>("spTrainingPlanExercise_GetAll", new { Id = id });

		TrainingPlanWithExercisesModel planWithExercises = new()
		{
			Plan = plan,
			PlanExercises = exercises.ToList()
		};

		return planWithExercises;
	}

	public async Task InsertPlan(TrainingPlanModel model) =>
		await _db.SaveData("spTrainingPlan_Insert", new
		{
			model.Title,
			model.TrainingDays,
			model.Notes
		});

	public async Task UpdatePlan(TrainingPlanModel model) =>
		await _db.SaveData("spTrainingPlan_Update", model);

	public async Task DeletePlan(int id) =>
		await _db.SaveData("spTrainingPlan_Delete", new { Id = id });



	public async Task<TrainingPlanExerciseModel> GetPlanExercise(int id)
	{
		var exercises = await _db.GetData<TrainingPlanExerciseModel, dynamic>("spTrainingPlanExercise_Get", new { Id = id });
		return exercises.FirstOrDefault();
	}

	public async Task InsertPlanExercise(TrainingPlanExerciseModel model) =>
		await _db.SaveData("spTrainingPlanExercise_Insert", new
		{
			model.TPId,
			model.EId,
			model.Series,
			model.Weights
		});

	public async Task UpdatePlanExercise(TrainingPlanExerciseModel model) =>
		await _db.SaveData("spTrainingPlanExercise_Update", model);

	public async Task DeletePlanExercise(int id) =>
		await _db.SaveData("spTrainingPlanExercise_Delete", new { Id = id });
}
