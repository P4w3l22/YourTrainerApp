using YourTrainer_Utility;
using YourTrainerApp.Models;
using YourTrainerApp.Models.DTO;
using YourTrainer_App.Services.APIServices.IServices;

namespace YourTrainer_App.Services.APIServices;

public class TrainingPlanExerciseService : BaseService, ITrainingPlanExerciseService
{
    private readonly IHttpClientFactory _client;
    private string APIUrl;

    public TrainingPlanExerciseService(IHttpClientFactory client, IConfiguration configuration) : base(client)
    {
        _client = client;
        APIUrl = configuration.GetValue<string>("ServiceUrls:ExerciseAPI");
    }

    public Task<T> GetAllAsync<T>(int id) =>
        SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = APIUrl + "/api/TrainingPlanExercise/" + id
        });

    public Task<T> InsertAsync<T>(TrainingPlanExerciseCreateDTO trainingPlanExercise) =>
        SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.POST,
            Url = APIUrl + "/api/TrainingPlanExercise/",
            Data = trainingPlanExercise
        });

    public Task<T> UpdateAsync<T>(TrainingPlanExerciseUpdateDTO trainingPlanExercise) =>
        SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.PUT,
            Url = APIUrl + "/api/TrainingPlanExercise/",
            Data = trainingPlanExercise
        });

    public Task<T> DeleteAsync<T>(int id) =>
        SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = APIUrl + "/api/TrainingPlanExercise/" + id
        });

}
