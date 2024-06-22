using DbDataAccess.DbAccess;
using DbDataAccess.Models;
using YourTrainer_DBDataAccess.Models;

namespace DbDataAccess.Data;

public class AssignedTrainingPlanData : IAssignedTrainingPlanData
{
	private readonly ISqlDataAccess _db;

	public AssignedTrainingPlanData(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task<IEnumerable<AssignedTrainingPlan>> GetAssignedPlans(int trainerId) =>
		await _db.GetData<AssignedTrainingPlan, dynamic>("spAssignedTrainingPlans_GetAll", new { TrainerId = trainerId });

	public async Task<AssignedTrainingPlan> GetAssignedPlan(int clientId)
	{
		var exercise = await _db.GetData<AssignedTrainingPlan, dynamic>("spAssignedTrainingPlans_Get", new { ClientId = clientId });
		return exercise.FirstOrDefault();
	}

	public async Task InsertAssignedPlan(AssignedTrainingPlan assignedTrainingPlan) =>
		await _db.SaveData("spAssingedTrainingPlans_Insert", new
		{
			assignedTrainingPlan.TrainerId,
			assignedTrainingPlan.ClientId,
			assignedTrainingPlan.PlanId
		});


	public async Task DeleteAssignedPlan(int id) =>
		await _db.SaveData("spAssingedTrainingPlans_Delete", new { Id = id });
}
