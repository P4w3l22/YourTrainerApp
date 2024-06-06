using DbDataAccess.DbAccess;
using DbDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		return members.FirstOrDefault();
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