using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using YourTrainer_App.Repository.DataRepository;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainerApp.Areas.GymMember.Models;
using YourTrainerApp.Models;

namespace YourTrainerApp.Areas.GymMember.Controllers;

[Area("GymMember")]
public class TrainerContactController : Controller
{
	private readonly ITrainerClientDataRepository _trainerClientDataRepository;
	private int _memberId => int.Parse(HttpContext.Session.GetString("UserId"));

	public TrainerContactController(ITrainerClientDataRepository trainerClientDataRepository)
    {
		_trainerClientDataRepository = trainerClientDataRepository;
	}

	public async Task<IActionResult> Index()
	{
		MemberDataModel memberData = await _trainerClientDataRepository.GetMemberData(_memberId);

		if (memberData is not null && !memberData.TrainersId.IsNullOrEmpty() && memberData.TrainersId != "0")
		{
			return RedirectToAction("TrainerDetails", "TrainerContact", new { Area = "GymMember", trainerId = int.Parse(memberData.TrainersId) });
		}

		List<TrainerDataModel> trainersData = await _trainerClientDataRepository.GetTrainersOptions();

		return View(trainersData);
	}

	public async Task<IActionResult> TrainerDetails(int trainerId)
	{
		TrainerContact trainerContact = await _trainerClientDataRepository.GetTrainerDetails(trainerId, _memberId);

		return View(trainerContact);
	}

	public async Task<IActionResult> TrainerMessages()
	{
		
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}

	public async Task<IActionResult> SendMessage(string newMessage, int trainerId)
	{
		await _trainerClientDataRepository.SendMessage(newMessage, _memberId, trainerId);

		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}

	public async Task<IActionResult> AddTrainer(int id) 
	{
		await _trainerClientDataRepository.AddTrainerClientCooperation(id, _memberId);

		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}

	public async Task<IActionResult> DeleteTrainerAsync()
	{
		await _trainerClientDataRepository.DeleteTrainerClientCooperation(_memberId);

		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}
}
