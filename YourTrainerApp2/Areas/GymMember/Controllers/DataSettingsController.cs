using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models;
using YourTrainerApp.Services;
using YourTrainerApp.Services.IServices;

namespace YourTrainerApp.Areas.GymMember.Controllers;

[Area("GymMember")]
public class DataSettingsController : Controller
{
	private readonly IMemberDataService _memberDataService;

	private int _memberId
	{
		get => int.Parse(HttpContext.Session.GetString("UserId"));
	}


	public DataSettingsController(IMemberDataService memberDataService)
	{
		_memberDataService = memberDataService;
	}

	[HttpGet]
	public async Task<IActionResult> ShowData() =>
		await MemberDataIsPresent() ? View(await GetMemberDataFromDb()) : View(GetMemberDataDefault());

	[HttpPost]
	public async Task<IActionResult> ShowData(MemberDataModel memberData)
	{
		if (await MemberDataIsPresent())
		{
			await _memberDataService.UpdateAsync<APIResponse>(memberData);
		}
		else
		{
			await _memberDataService.CreateAsync<APIResponse>(memberData);
		}

		TempData["success"] = "Zapisano zmiany";
		return RedirectToAction("ShowData");
	}

	public async Task<IActionResult> ClearData()
	{
		// TODO: Usunąć wszystkie dane poza przyporządkowanymi trenerami

		APIResponse apiResponse = await _memberDataService.DeleteAsync<APIResponse>(_memberId);

		return RedirectToAction("ShowData");
	}

	private async Task<bool> MemberDataIsPresent()
	{
		APIResponse apiResponse = await _memberDataService.GetAsync<APIResponse>(_memberId);
		return apiResponse.Result is not null;
	}

	private async Task<MemberDataModel> GetMemberDataFromDb()
	{
		APIResponse apiResponse = await _memberDataService.GetAsync<APIResponse>(_memberId);
		return JsonConvert.DeserializeObject<MemberDataModel>(Convert.ToString(apiResponse.Result));
	}

	private MemberDataModel GetMemberDataDefault() =>
		new MemberDataModel()
		{
			MemberId = _memberId,
			Email = HttpContext.Session.GetString("Username")
		};
}
