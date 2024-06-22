using DbDataAccess.Models;

namespace YourTrainer_DBDataAccess.Data.IData;

public interface ITrainerClientContactData
{
    Task<IEnumerable<TrainerClientContact>> GetMessages(int senderId, int receiverId, string messageType);
    Task<IEnumerable<TrainerClientContact>> GetCooperationProposals(int receiverId, string messageType);
    Task SendMessage(TrainerClientContact trainerClientContact);
    Task SetAsRead(int id);
}