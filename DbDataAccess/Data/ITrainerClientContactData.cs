using DbDataAccess.Models;

namespace DbDataAccess.Data;

public interface ITrainerClientContactData
{
	Task<IEnumerable<TrainerClientContact>> GetMessages(int senderId, int receiverId, string messageType);
	Task SendMessage(TrainerClientContact trainerClientContact);
	Task SetAsRead(int id);
}