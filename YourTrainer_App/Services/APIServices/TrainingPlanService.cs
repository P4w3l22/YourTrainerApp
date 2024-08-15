using YourTrainer_Utility;
using YourTrainer_App.Models;
using YourTrainer_App.Services.APIServices.IServices;

namespace YourTrainer_App.Services.APIServices;

public class TrainingPlanService : BaseService, ITrainingPlanService
{
    private readonly IHttpClientFactory _client;
    private string? APIUrl;

    public TrainingPlanService(IHttpClientFactory client, IConfiguration? configuration) : base(client)
    {
        _client = client;
        APIUrl = configuration?.GetValue<string>("ServiceUrls:YourTrainer_API");
    }

    public Task<T> GetAllAsync<T>() =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = APIUrl + "/api/TrainingPlan/"
        });

    public Task<T> GetAsync<T>(int id) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = APIUrl + "/api/TrainingPlan/" + id
        });


    public Task<T> CreateAsync<T>(TrainingPlan trainingPlan) =>
        SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.POST,
            Url = APIUrl + "/api/TrainingPlan/",
            Data = trainingPlan
        });

    public Task<T> UpdateAsync<T>(TrainingPlan trainingPlan) =>
        SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.PUT,
            Url = APIUrl + "/api/TrainingPlan/",
            Data = trainingPlan
        });

    public Task<T> DeleteAsync<T>(int id) =>
        SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = APIUrl + "/api/TrainingPlan/" + id
        });
}
