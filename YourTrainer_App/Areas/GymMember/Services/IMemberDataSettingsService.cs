using YourTrainer_App.Models;

namespace YourTrainer_App.Areas.GymMember.Services;

public interface IMemberDataSettingsService
{
	Task<string> UpdateMemberDataOrGetErrorResponse(MemberDataModel memberData);
	Task<string> CreateMemberDataOrGetErrorResponse(MemberDataModel memberData, string sessionToken);
	Task ClearMemberData(int memberId, string sessionToken);
	Task<MemberDataModel> GetMemberDataFromDb(int memberId);
	Task<bool> MemberDataIsPresent(int memberId);
	MemberDataModel GetMemberDataDefault(int memberId, string username);
}