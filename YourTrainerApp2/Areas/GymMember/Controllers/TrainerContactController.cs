using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_App.Services.DataServices;
using YourTrainer_Utility;
using YourTrainerApp.Areas.GymMember.Models;
using YourTrainerApp.Models;

namespace YourTrainerApp.Areas.GymMember.Controllers;

[Area("GymMember")]
public class TrainerContactController : Controller
{
	private readonly ITrainerClientDataService _trainerClientDataService;
	private int _memberId => int.Parse(HttpContext.Session.GetString("UserId"));

	public TrainerContactController(ITrainerClientDataService trainerClientDataService)
    {
		_trainerClientDataService = trainerClientDataService;
	}


	public async Task<IActionResult> Index()
	{
		MemberDataModel memberData = await _trainerClientDataService.GetMemberData(_memberId);

		if (memberData is not null && !memberData.TrainersId.IsNullOrEmpty() && memberData.TrainersId != "0")
		{
			return RedirectToAction("TrainerDetails", "TrainerContact", new { Area = "GymMember", trainerId = int.Parse(memberData.TrainersId) });
		}

		List<TrainerDataModel> trainersData = await _trainerClientDataService.GetTrainersOptions();
		return View(trainersData);
	}


	public async Task<IActionResult> TrainerDetails(int trainerId)
	{
		TrainerContact trainerContact = await _trainerClientDataService.GetTrainerDetails(trainerId, _memberId);
		return View(trainerContact);
	}


	public async Task<IActionResult> TrainerMessages() =>
		RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	

	public async Task<IActionResult> SendMessage(string newMessage, int trainerId)
	{
		await _trainerClientDataService.SendMessage(newMessage, _memberId, trainerId);
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}


	public async Task<IActionResult> AddTrainer(int id) 
	{
		await _trainerClientDataService.AddTrainerClientCooperation(id, _memberId);
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}


	public async Task<IActionResult> DeleteTrainerAsync()
	{
		await _trainerClientDataService.DeleteTrainerClientCooperation(_memberId);
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}
}
