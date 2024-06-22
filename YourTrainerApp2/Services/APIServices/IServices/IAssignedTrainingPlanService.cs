using YourTrainerApp.Models;

namespace YourTrainer_App.Services.APIServices.IServices;

public interface IAssignedTrainingPlanService
{
    Task<T> CreateAsync<T>(AssignedTrainingPlan assignedTrainingPlan, string token);
    Task<T> DeleteAsync<T>(int id, string token);
    Task<T> GetAllAsync<T>(int trainerId);
    Task<T> GetAsync<T>(int clientId);
}