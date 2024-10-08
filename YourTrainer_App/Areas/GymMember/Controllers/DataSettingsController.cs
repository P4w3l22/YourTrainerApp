﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourTrainer_App.Areas.GymMember.Services;
using YourTrainer_Utility;
using YourTrainer_App.Models;

namespace YourTrainer_App.Areas.GymMember.Controllers;

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
			string errorResponse;
			if (await _memberDataSettingsService.MemberDataIsPresent(_memberId))
			{
				errorResponse = await _memberDataSettingsService.UpdateMemberDataOrGetErrorResponse(memberData);
			}
			else
			{
				errorResponse = await _memberDataSettingsService.CreateMemberDataOrGetErrorResponse(memberData, HttpContext.Session.GetString(StaticDetails.SessionToken));
			}

			if (errorResponse.Length > 0)
			{
				ModelState.AddModelError(string.Empty, errorResponse);
			}
			else
			{
				TempData["success"] = "Zapisano zmiany";
				return RedirectToAction("ShowData");
			}
		}

		return View(memberData);
	}

	[Authorize(Roles = "gym member")]
	public async Task<IActionResult> ClearData()
	{
		await _memberDataSettingsService.ClearMemberData(_memberId, HttpContext.Session.GetString(StaticDetails.SessionToken));
		return RedirectToAction("ShowData");
	}
}
