using YourTrainer_DBDataAccess.DbAccess;
using YourTrainer_DBDataAccess.Models;
using YourTrainer_DBDataAccess.Data.IData;

namespace YourTrainer_DBDataAccess.Data;

public class TrainerClientContactData : ITrainerClientContactData
{
	private readonly ISqlDataAccess _db;

	public TrainerClientContactData(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task<IEnumerable<TrainerClientContact>> GetMessages(int senderId, int receiverId, string messageType) =>
		await _db.GetData<TrainerClientContact, dynamic>("spTrainerClientContact_GetMessages", new 
		{ 
			SenderId = senderId,
			ReceiverId = receiverId, 
			MessageType = messageType 
		});

	public async Task<IEnumerable<TrainerClientContact>> GetCooperationProposals(int receiverId, string messageType) =>
		await _db.GetData<TrainerClientContact, dynamic>("spTrainerClientContact_GetCooperationProposals", new { ReceiverId = receiverId, MessageType = messageType });

	public async Task SendMessage(TrainerClientContact trainerClientContact) =>
		await _db.SaveData("spTrainerClientContact_SendMessage", new
		{
			trainerClientContact.SenderId,
			trainerClientContact.ReceiverId,
			trainerClientContact.MessageType,
			trainerClientContact.MessageContent
		});

	public async Task SetAsRead(int id) =>
		await _db.SaveData("spTrainerClientContact_SetAsRead", new { Id = id });

}
