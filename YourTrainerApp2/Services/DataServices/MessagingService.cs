using YourTrainer_App.Services.APIServices.IServices;
using YourTrainerApp.Models;
using static YourTrainer_Utility.StaticDetails;

namespace YourTrainer_App.Services.DataServices;

public class MessagingService : IMessagingService
{
	private readonly ITrainerClientContactService _trainerClientContactService;

	public MessagingService(ITrainerClientContactService trainerClientContactService)
	{
		_trainerClientContactService = trainerClientContactService;
	}

	//public async Task SendMessage(string newMessage, int senderId, int receiverId)
	//{
	//	if (!string.IsNullOrEmpty(newMessage))
	//	{
	//		TrainerClientContact messageToSend = new()
	//		{
	//			Id = 0,
	//			SenderId = senderId,
	//			ReceiverId = receiverId,
	//			MessageType = MessageType.Text.ToString(),
	//			MessageContent = newMessage
	//		};
	//		await _trainerClientContactService.SendMessageAsync<APIResponse>(messageToSend);
	//	}
	//}

	//private
	public async Task SendMessage(string newMessage, int senderId, int receiverId, string messageType)
	{
		if (!string.IsNullOrEmpty(newMessage))
		{
			TrainerClientContact messageToSend = new()
			{
				Id = 0,
				SenderId = senderId,
				ReceiverId = receiverId,
				MessageType = messageType,
				MessageContent = newMessage
			};
			await _trainerClientContactService.SendMessageAsync<APIResponse>(messageToSend);
		}
	}
}
