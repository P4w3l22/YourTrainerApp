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
using static YourTrainer_Utility.StaticDetails;

namespace YourTrainerApp.Areas.GymMember.Controllers;

[Area("GymMember")]
public class TrainerContactController : Controller
{
	private readonly ICooperationProposalService _cooperationProposalService;
	private readonly ITrainerClientDataService _trainerClientDataService;
	private readonly IMessagingService _messagingService;
	private int _memberId => int.Parse(HttpContext.Session.GetString("UserId"));

	public TrainerContactController(ICooperationProposalService cooperationProposalService, ITrainerClientDataService trainerClientDataService, IMessagingService messagingService)
    {
		_cooperationProposalService = cooperationProposalService;
		_trainerClientDataService = trainerClientDataService;
		_messagingService = messagingService;
	}


	public async Task<IActionResult> Index()
	{
		MemberDataModel memberData = await _trainerClientDataService.GetMemberData(_memberId);

		if (memberData is not null && !memberData.TrainersId.IsNullOrEmpty() && memberData.TrainersId != "0" && memberData.TrainersId != "-1")
		{
			// sprawdzić zawartość wiadomości, jeśli odpowiedź pozytywna - wyświetl że trener dodał, jeśli nie - że odmówił

			return RedirectToAction("TrainerDetails", "TrainerContact", new { Area = "GymMember", trainerId = int.Parse(memberData.TrainersId) });
		}

		if (memberData.TrainersId == "-1")
		{
			HttpContext.Session.SetString("WaitingForAnAnswer", "True");
		}
		else
		{
			HttpContext.Session.SetString("WaitingForAnAnswer", "");
		}
		
		string trainerResponseResult = await _cooperationProposalService.GetCooperationProposalResponse(_memberId);
		if (trainerResponseResult == "Rejected")
		{
			TempData["error"] = "Trener odmówił współpracy";
		}

		List<TrainerDataModel> trainersData = await _trainerClientDataService.GetTrainersOptions();
		return View(trainersData);
	}

	public async Task<IActionResult> TrainerDetails(int trainerId)
	{
		TrainerContact trainerContact = await _trainerClientDataService.GetTrainerDetails(trainerId, _memberId);

		string trainerResponseResult = await _cooperationProposalService.GetCooperationProposalResponse(_memberId);

		if (trainerResponseResult == "Accepted")
		{
			TempData["success"] = "Trener zaakceptował Twoją propozycję!";
		}

		return View(trainerContact);
	}


	public IActionResult TrainerMessages() =>
		RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	

	public async Task<IActionResult> SendMessage(string newMessage, int trainerId)
	{
		await _messagingService.SendMessage(newMessage, _memberId, trainerId, MessageType.Text.ToString());
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}


	public async Task<IActionResult> AddTrainer(int id) 
	{
		await _cooperationProposalService.SendCooperationProposal(id, _memberId);
		TempData["success"] = "Wysłano wiadomość do trenera odnośnie chęci współpracy";
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}


	public async Task<IActionResult> DeleteTrainerAsync()
	{
		await _cooperationProposalService.DeleteTrainerClientCooperation(_memberId);
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}
}
