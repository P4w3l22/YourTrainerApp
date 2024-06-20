using YourTrainerApp.Models;

namespace YourTrainer_App.Services.APIServices.IServices;

public interface ITrainerClientContactService
{
    Task<T> GetMessagesAsync<T>(int senderId, int receiverId, string messageType);
	Task<T> GetCooperationProposals<T>(int receiverId, string messageType);
	Task<T> SendMessageAsync<T>(TrainerClientContact trainerClientContactSend);
    Task<T> SetAsReadAsync<T>(int id);
}