using YourTrainerApp.Models.DTO;

namespace YourTrainerApp.Services.IServices;

public interface ITrainingPlanExerciseService
{
    Task<T> DeleteAsync<T>(int id);
    Task<T> GetAllAsync<T>(int id);
    Task<T> InsertAsync<T>(TrainingPlanExerciseCreateDTO trainingPlanExercise);
    Task<T> UpdateAsync<T>(TrainingPlanExerciseUpdateDTO trainingPlanExercise);
}