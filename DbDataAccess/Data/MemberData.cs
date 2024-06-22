using DbDataAccess.DbAccess;
using DbDataAccess.Models;
using YourTrainer_DBDataAccess.Data.IData;

namespace DbDataAccess.Data;

public class MemberData : IMemberData
{
	private readonly ISqlDataAccess _db;

	public MemberData(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task<IEnumerable<MemberDataModel>> GetMembers() =>
		await _db.GetData<MemberDataModel, dynamic>("spMemberData_GetAll", new { });

	public async Task<MemberDataModel> GetMember(int id)
	{
		var members = await _db.GetData<MemberDataModel, dynamic>("spMemberData_Get", new { MemberId = id });
		return members.FirstOrDefault() ?? throw new InvalidOperationException("Brak użytkownika o tym id w bazie danych");
	}

	public async Task InsertMemberData(MemberDataModel memberData) =>
		await _db.SaveData("spMemberData_Insert", new
		{
			memberData.MemberId,
			memberData.MemberName,
			memberData.Description,
			memberData.Email,
			memberData.PhoneNumber,
			memberData.TrainersId,
			memberData.TrainersPlan
		});

	public async Task UpdateMemberData(MemberDataModel memberData) =>
		await _db.SaveData("spMemberData_Update", new
		{
			memberData.MemberId,
			memberData.MemberName,
			memberData.Description,
			memberData.Email,
			memberData.PhoneNumber,
			memberData.TrainersId,
			memberData.TrainersPlan
		});

	public async Task DeleteMemberData(int id) =>
		await _db.SaveData("spMemberData_Delete", new { MemberId = id });
}