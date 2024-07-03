using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourTrainer_App.Areas.GymMember.Services;
using YourTrainerApp.Models;

namespace YourTrainerApp.Areas.GymMember.Controllers;

[Area("GymMember")]
public class DataSettingsController : Controller
{
	private readonly IMemberDataSettingsService _memberDataSettingsService;
	private int _memberId => int.Parse(HttpContext.Session.GetString("UserId"));

	public DataSettingsController(IMemberDataSettingsService memberDataSettingsService)
	{
		_memberDataSettingsService = memberDataSettingsService;
	}

	[HttpGet]
	[Authorize(Roles = "gym member")]
	public async Task<IActionResult> ShowData()
	{
		if (await _memberDataSettingsService.MemberDataIsPresent(_memberId))
		{
			return View(await _memberDataSettingsService.GetMemberDataFromDb(_memberId));
		}

		return View(_memberDataSettingsService.GetMemberDataDefault(_memberId, HttpContext.Session.GetString("Username")));
	}

	[HttpPost]
	[Authorize(Roles = "gym member")]
	public async Task<IActionResult> ShowData(MemberDataModel memberData)
	{
		if (memberData.TrainersId is null)
		{
			memberData.TrainersId = "0";
		}

		if (memberData.TrainersPlan is null)
		{
			memberData.TrainersPlan = "0";
		}

		if (ModelState.IsValid)
		{
			if (await _memberDataSettingsService.MemberDataIsPresent(_memberId))
			{
				await _memberDataSettingsService.UpdateMemberData(memberData);
			}
			else
			{
				await _memberDataSettingsService.CreateMemberData(memberData);
			}

			TempData["success"] = "Zapisano zmiany";
			return RedirectToAction("ShowData");
		}

		return View(memberData);
	}

	[Authorize(Roles = "gym member")]
	public async Task<IActionResult> ClearData()
	{
		await _memberDataSettingsService.ClearMemberData(_memberId);
		return RedirectToAction("ShowData");
	}
}
