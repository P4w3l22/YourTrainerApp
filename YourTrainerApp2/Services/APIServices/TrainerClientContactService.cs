using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainerApp.Models;

namespace YourTrainer_App.Services.APIServices;

public class TrainerClientContactService : BaseService, ITrainerClientContactService
{
    private string? APIUrl;
    public TrainerClientContactService(IHttpClientFactory client, IConfiguration configuration) : base(client)
    {
        APIUrl = configuration.GetValue<string>("ServiceUrls:ExerciseAPI");
    }

    public Task<T> GetMessagesAsync<T>(int senderId, int receiverId, string messageType) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = APIUrl + "/api/TrainerClientContact/" + senderId + "/" + receiverId + "/" + messageType
        });

	public Task<T> GetCooperationProposals<T>(int receiverId, string messageType) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.GET,
			Url = APIUrl + "/api/TrainerClientContact/" + receiverId + "/" + messageType
		});

	public Task<T> SendMessageAsync<T>(TrainerClientContact trainerClientContactSend) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Url = APIUrl + "/api/TrainerClientContact/",
            Data = trainerClientContactSend
            //Token = token
        });

    public Task<T> SetAsReadAsync<T>(int id) =>
        SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.PUT,
            Url = APIUrl + "/api/TrainerClientContact/" + id,
            //Token = token
        });
}
