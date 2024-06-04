using YourTrainerApp.Models;

namespace YourTrainerApp.Services.IServices;

public interface ITrainerDataService
{
    Task<T> CreateAsync<T>(TrainerDataModel trainerData);
    Task<T> DeleteAsync<T>(int id);
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> UpdateAsync<T>(TrainerDataModel trainerData);
}