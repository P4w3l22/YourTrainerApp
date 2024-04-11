
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

	//TODO: dodać opcjonalny warunek po którym będzie można wyszukać plan
	//TODO: dodać do tabeli trainingPlan kolumnę określającą właściciela planu
	public async Task<IEnumerable<TrainingPlanModel>> GetAllPlans() =>
		await _db.GetData<TrainingPlanModel, dynamic>("dbo.spTrainingPlan_GetAll", new { });

	public async Task<TrainingPlanModel> GetPlan(int id)
	{
		var plans = await _db.GetData<TrainingPlanModel, dynamic>("spTrainingPlan_Get", new { Id = id });
		var plan = plans.FirstOrDefault();

		//var exercises = await _db.GetData<TrainingPlanExerciseModel, dynamic>("spTrainingPlanExercises_GetAll", new { TPId = id });

		//TrainingPlanWithExercisesModel planWithExercises = new()
		//{
		//	Plan = plan,
		//	PlanExercises = exercises.ToList()
		//};

		return plan;
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


	public async Task<IEnumerable<TrainingPlanExerciseModel>> GetPlanExercises(int id) =>
		await _db.GetData<TrainingPlanExerciseModel, dynamic>("spTrainingPlanExercises_GetAll", new { TPId = id });


	//public async Task<TrainingPlanExerciseModel> GetPlanExercises(int id)
	//{
	//	var exercises = await _db.GetData<TrainingPlanExerciseModel, dynamic>("spTrainingPlanExercises_GetAll", new { TPId = id });
	//	return exercises.FirstOrDefault();
	//}

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
