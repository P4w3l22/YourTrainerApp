using YourTrainerApp.Models;

namespace YourTrainer_App.Services.APIServices.IServices;

public interface IExerciseService
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> CreateAsync<T>(Exercise exercise, string token);
    Task<T> UpdateAsync<T>(Exercise exercise, string token);
    Task<T> DeleteAsync<T>(int id, string token);
}
