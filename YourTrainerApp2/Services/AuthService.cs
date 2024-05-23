using YourTrainer_Utility;
using YourTrainerApp.Areas.Auth.Models;
using YourTrainerApp2.Models;
using YourTrainerApp2.Services;

namespace YourTrainerApp.Services.IServices;

public class AuthService : BaseService, IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private string APIUrl;
    
    public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
    {
        _clientFactory = clientFactory;
        APIUrl = configuration.GetValue<string>("ServiceUrls:ExerciseAPI");
    }

    public Task<T> LoginAsync<T>(LoginRequest loginRequest) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Url = APIUrl + "/api/UserAuth/Login",
            Data = loginRequest
        });

    public Task<T> RegisterAsync<T>(RegisterationRequest userData) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Url = APIUrl + "/api/UserAuth/Register",
            Data = userData
        });

}