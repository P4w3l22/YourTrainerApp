using Newtonsoft.Json;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainerApp.Areas.Admin.Controllers;
using YourTrainerApp.Models;

namespace YourTrainer_App.Areas.GymMember.Services;

public class MemberDataSettingsService : IMemberDataSettingsService
{
	private readonly ILogger<ExerciseAdminController> _logger;
	private readonly IMemberDataService _memberDataService;

	public MemberDataSettingsService(IMemberDataService memberDataService, ILogger<ExerciseAdminController> logger)
	{
		_memberDataService = memberDataService;
		_logger = logger;
	}

	public async Task UpdateMemberData(MemberDataModel memberData) =>
		await _memberDataService.UpdateAsync<APIResponse>(memberData);

	public async Task CreateMemberData(MemberDataModel memberData) =>
		await _memberDataService.CreateAsync<APIResponse>(memberData);

	public async Task ClearMemberData(int memberId) =>
		await _memberDataService.DeleteAsync<APIResponse>(memberId);

	public async Task<bool> MemberDataIsPresent(int memberId)
	{
		APIResponse apiResponse = await _memberDataService.GetAsync<APIResponse>(memberId);
		return apiResponse.Result is not null;
	}

	public async Task<MemberDataModel> GetMemberDataFromDb(int memberId)
	{
		APIResponse apiResponse = await _memberDataService.GetAsync<APIResponse>(memberId);
		return JsonConvert.DeserializeObject<MemberDataModel>(Convert.ToString(apiResponse.Result));
	}

	public MemberDataModel GetMemberDataDefault(int memberId, string username) =>
	new()
	{
		MemberId = memberId,
		Email = username
	};
}
