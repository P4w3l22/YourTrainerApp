using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainer_App.Models;

namespace YourTrainer_App.Services.APIServices;

public class ExerciseService : BaseService, IExerciseService
{
    private string? APIUrl;
    public ExerciseService(IHttpClientFactory client, IConfiguration configuration) : base(client)
    {
        APIUrl = configuration.GetValue<string>("ServiceUrls:YourTrainer_API");
    }

    public Task<T> GetAllAsync<T>() =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = APIUrl + "/api/YourTrainer_API/"
        });

    public Task<T> GetAsync<T>(int id) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = APIUrl + "/api/YourTrainer_API/" + id
        });

    public Task<T> CreateAsync<T>(Exercise exercise, string token) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Url = APIUrl + "/api/YourTrainer_API/",
            Data = exercise,
            Token = token
        });

    public Task<T> UpdateAsync<T>(Exercise exercise, string token) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.PUT,
            Url = APIUrl + "/api/YourTrainer_API/",
            Data = exercise,
            Token = token
        });

    public Task<T> DeleteAsync<T>(int id, string token) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = APIUrl + "/api/YourTrainer_API/" + id,
            Token = token
        });

}
