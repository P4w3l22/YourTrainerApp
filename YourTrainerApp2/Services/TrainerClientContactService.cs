using YourTrainer_Utility;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;

namespace YourTrainerApp.Services;

public class TrainerClientContactService : BaseService, ITrainerClientContactService
{
	private string APIUrl;
	public TrainerClientContactService(IHttpClientFactory client, IConfiguration configuration) : base(client)
	{
		APIUrl = configuration.GetValue<string>("ServiceUrls:ExerciseAPI");
	}

	public Task<T> GetMessagesAsync<T>(int receiverId, string messageType) =>
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
