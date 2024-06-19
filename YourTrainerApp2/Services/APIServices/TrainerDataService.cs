using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainerApp.Models;

namespace YourTrainer_App.Services.APIServices;

public class TrainerDataService : BaseService, ITrainerDataService
{
    private string APIUrl;
    public TrainerDataService(IHttpClientFactory client, IConfiguration configuration) : base(client)
    {
        APIUrl = configuration.GetValue<string>("ServiceUrls:ExerciseAPI");
    }

    public Task<T> GetAllAsync<T>() =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = APIUrl + "/api/TrainerData/"
        });

    public Task<T> GetAsync<T>(int id) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = APIUrl + "/api/TrainerData/" + id
        });

    public Task<T> CreateAsync<T>(TrainerDataModel trainerData) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Url = APIUrl + "/api/TrainerData/",
            Data = trainerData
        });

    public Task<T> UpdateAsync<T>(TrainerDataModel trainerData) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.PUT,
            Url = APIUrl + "/api/TrainerData/",
            Data = trainerData
        });

    public Task<T> DeleteAsync<T>(int id) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = APIUrl + "/api/TrainerData/" + id
        });
}
