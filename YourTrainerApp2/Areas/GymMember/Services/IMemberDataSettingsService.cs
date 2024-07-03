using YourTrainerApp.Models;

namespace YourTrainer_App.Areas.GymMember.Services;

public interface IMemberDataSettingsService
{
	Task UpdateMemberData(MemberDataModel memberData);
	Task CreateMemberData(MemberDataModel memberData);
	Task ClearMemberData(int memberId);
	Task<MemberDataModel> GetMemberDataFromDb(int memberId);
	Task<bool> MemberDataIsPresent(int memberId);
	MemberDataModel GetMemberDataDefault(int memberId, string username);
}