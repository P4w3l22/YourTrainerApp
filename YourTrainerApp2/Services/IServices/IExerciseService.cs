using YourTrainerApp2.Models;

namespace YourTrainerApp2.Services.IServices
{
    public interface IExerciseService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(Exercise exercise);
        Task<T> UpdateAsync<T>(Exercise exercise);
        Task<T> DeleteAsync<T>(int id);
    }
}
