using YourTrainerApp.Models;

namespace YourTrainerApp.Services.IServices;

public interface IMemberDataService
{
    Task<T> CreateAsync<T>(MemberDataModel memberData);
    Task<T> DeleteAsync<T>(int id);
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> UpdateAsync<T>(MemberDataModel memberData);
}