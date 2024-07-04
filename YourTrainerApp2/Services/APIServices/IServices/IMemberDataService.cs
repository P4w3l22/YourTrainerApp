using YourTrainerApp.Models;

namespace YourTrainer_App.Services.APIServices.IServices;

public interface IMemberDataService
{
    Task<T> CreateAsync<T>(MemberDataModel memberData, string token);
    Task<T> DeleteAsync<T>(int id, string token);
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> UpdateAsync<T>(MemberDataModel memberData);
}