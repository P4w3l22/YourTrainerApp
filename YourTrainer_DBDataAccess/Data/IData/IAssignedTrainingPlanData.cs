using YourTrainer_DBDataAccess.Models;
using YourTrainer_DBDataAccess.Models;

namespace YourTrainer_DBDataAccess.Data.IData;

public interface IAssignedTrainingPlanData
{
    Task DeleteAssignedPlan(int id);
    Task<AssignedTrainingPlan> GetAssignedPlan(int clientId);
    Task<IEnumerable<AssignedTrainingPlan>> GetAssignedPlans(int trainerId);
    Task InsertAssignedPlan(AssignedTrainingPlan assignedTrainingPlan);
}