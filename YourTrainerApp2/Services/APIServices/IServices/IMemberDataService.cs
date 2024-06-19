using YourTrainerApp.Models;

namespace YourTrainer_App.Services.APIServices.IServices;

public interface IMemberDataService
{
    Task<T> CreateAsync<T>(MemberDataModel memberData);
    Task<T> DeleteAsync<T>(int id);
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> UpdateAsync<T>(MemberDataModel memberData);
}