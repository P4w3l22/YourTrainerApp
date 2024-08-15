
namespace YourTrainer_App.Services.DataServices
{
	public interface IMessagingService
	{
		Task SendMessage(string newMessage, int senderId, int receiverId, string messageType);
	}
}