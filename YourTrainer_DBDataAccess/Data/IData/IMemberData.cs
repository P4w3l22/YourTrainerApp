using YourTrainer_DBDataAccess.Models;

namespace YourTrainer_DBDataAccess.Data.IData;

public interface IMemberData
{
    Task DeleteMemberData(int id);
    Task<MemberDataModel> GetMember(int id);
    Task<IEnumerable<MemberDataModel>> GetMembers();
    Task InsertMemberData(MemberDataModel memberData);
    Task UpdateMemberData(MemberDataModel memberData);
}