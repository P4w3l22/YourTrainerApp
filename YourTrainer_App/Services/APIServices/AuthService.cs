using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainer_App.Areas.Auth.Models;
using YourTrainer_App.Models;
using LoginRequest = YourTrainer_App.Areas.Auth.Models.LoginRequest;


namespace YourTrainer_App.Services.APIServices;

public class AuthService : BaseService, IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private string? APIUrl;

    public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
    {
        _clientFactory = clientFactory;
        APIUrl = configuration.GetValue<string>("ServiceUrls:YourTrainer_API");
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