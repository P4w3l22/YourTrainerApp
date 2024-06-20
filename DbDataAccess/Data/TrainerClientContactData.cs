using DbDataAccess.DbAccess;
using DbDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDataAccess.Data;

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

	public async Task<IEnumerable<TrainerClientContact>> GetCooperationProposals(int receiverId) =>
		await _db.GetData<TrainerClientContact, dynamic>("spTrainerClientContact_GetCooperationProposals", new { ReceiverId = receiverId });

	public async Task SendMessage(TrainerClientContact trainerClientContact) =>
		await _db.SaveData("spTrainerClientContact_SendMessage", new
		{
			SenderId = trainerClientContact.SenderId,
			ReceiverId = trainerClientContact.ReceiverId,
			MessageType = trainerClientContact.MessageType,
			MessageContent = trainerClientContact.MessageContent
		});

	public async Task SetAsRead(int id) =>
		await _db.SaveData("spTrainerClientContact_SetAsRead", new { Id = id });

}
