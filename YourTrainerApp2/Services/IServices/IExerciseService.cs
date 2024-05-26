using YourTrainerApp.Models;

namespace YourTrainerApp.Services.IServices;

public interface IExerciseService
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> CreateAsync<T>(Exercise exercise);
    Task<T> UpdateAsync<T>(Exercise exercise);
    Task<T> DeleteAsync<T>(int id);
}
