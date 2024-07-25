using Newtonsoft.Json;
using System.Net;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_App.Services.DataServices;
using YourTrainerApp.Areas.Admin.Controllers;
using YourTrainerApp.Models;

namespace YourTrainer_App.Areas.GymMember.Services;

public class MemberDataSettingsService : IMemberDataSettingsService
{
	private readonly ILogger<ExerciseAdminController> _logger;
	private readonly IMemberDataService _memberDataService;
	private readonly ICooperationProposalService _cooperationProposalService;

	public MemberDataSettingsService(IMemberDataService memberDataService, ILogger<ExerciseAdminController> logger, ICooperationProposalService cooperationProposalService)
	{
		_memberDataService = memberDataService;
		_logger = logger;
		_cooperationProposalService = cooperationProposalService;
	}

	public async Task<string> UpdateMemberDataOrGetErrorResponse(MemberDataModel memberData)
	{
		APIResponse apiResponse = await _memberDataService.UpdateAsync<APIResponse>(memberData);
		return GetErrorResponse(apiResponse);
	}

	public async Task<string> CreateMemberDataOrGetErrorResponse(MemberDataModel memberData, string sessionToken)
	{
		APIResponse apiResponse = await _memberDataService.CreateAsync<APIResponse>(memberData, sessionToken);
		return GetErrorResponse(apiResponse);
	}

	private string GetErrorResponse(APIResponse? apiResponse)
	{
		if (apiResponse is not null)
		{
			if (apiResponse.StatusCode == HttpStatusCode.OK ||
				apiResponse.StatusCode == HttpStatusCode.Created ||
				apiResponse.StatusCode == HttpStatusCode.NoContent)
			{
				return string.Empty;
			}
			else if (apiResponse.StatusCode == HttpStatusCode.InternalServerError)
			{
				_logger.LogError(apiResponse.Errors.FirstOrDefault(), string.Empty);
			}
			else
			{
				return apiResponse.Errors.FirstOrDefault();
			}
		}
		else
		{
			_logger.LogError("apiResponse zwróciło null", string.Empty);
		}

		return "Wystąpił błąd podczas przetwarzania Twojego żądania. Spróbuj ponownie później.";
	}

	public async Task<string> ClearMemberData(int memberId, string sessionToken)
	{
		await _cooperationProposalService.DeleteTrainerClientCooperation(memberId);
		APIResponse apiResponse = await _memberDataService.DeleteAsync<APIResponse>(memberId, sessionToken);
		return GetErrorResponse(apiResponse);
	}

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
