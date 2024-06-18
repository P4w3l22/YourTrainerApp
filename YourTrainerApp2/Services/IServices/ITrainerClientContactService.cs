using YourTrainerApp.Models;

namespace YourTrainerApp.Services.IServices;

public interface ITrainerClientContactService
{
    Task<T> GetMessagesAsync<T>(int senderId, int receiverId, string messageType);
    Task<T> SendMessageAsync<T>(TrainerClientContact trainerClientContactSend);
    Task<T> SetAsReadAsync<T>(int id);
}