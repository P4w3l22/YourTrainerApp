using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourTrainer_App.Areas.GymMember.Services;
using YourTrainer_App.Services.DataServices;
using YourTrainer_App.Areas.GymMember.Models;
using YourTrainer_App.Models;
using static YourTrainer_Utility.StaticDetails;

namespace YourTrainer_App.Areas.GymMember.Controllers;

[Area("GymMember")]
public class TrainerContactController : Controller
{
	private readonly ICooperationProposalService _cooperationProposalService;
	private readonly ITrainerClientDataService _trainerClientDataService;
	private readonly IMessagingService _messagingService;
	private readonly IMemberDataSettingsService _memberDataSettingsService;
	private int _memberId => int.Parse(HttpContext.Session.GetString("UserId"));

	public TrainerContactController(ICooperationProposalService cooperationProposalService, ITrainerClientDataService trainerClientDataService, IMessagingService messagingService, IMemberDataSettingsService memberDataSettingsService)
    {
		_cooperationProposalService = cooperationProposalService;
		_trainerClientDataService = trainerClientDataService;
		_messagingService = messagingService;
		_memberDataSettingsService = memberDataSettingsService;
	}

	[Authorize(Roles = "gym member")]
	public async Task<IActionResult> Index()
	{
		if (!await _memberDataSettingsService.MemberDataIsPresent(_memberId))
		{
			TempData["error"] = "Najpierw uzupełnij swoje dane";
			return RedirectToAction("ShowData", "DataSettings", new { Area = "GymMember" });
		}

		MemberDataModel memberData = await _trainerClientDataService.GetMemberData(_memberId);

		if (_trainerClientDataService.TrainerIsAssigned(memberData))
		{
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

	[Authorize(Roles = "gym member")]
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

	[Authorize(Roles = "gym member")]
	public IActionResult TrainerMessages() =>
		RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });

	[Authorize(Roles = "gym member")]
	public async Task<IActionResult> SendMessage(string newMessage, int trainerId)
	{
		await _messagingService.SendMessage(newMessage, _memberId, trainerId, MessageType.Text.ToString());
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}

	[Authorize(Roles = "gym member")]
	public async Task<IActionResult> AddTrainer(int id) 
	{
		await _cooperationProposalService.SendCooperationProposal(id, _memberId);
		TempData["success"] = "Wysłano wiadomość do trenera odnośnie chęci współpracy";
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}

	[Authorize(Roles = "gym member")]
	public async Task<IActionResult> DeleteTrainerAsync()
	{
		await _cooperationProposalService.DeleteTrainerClientCooperation(_memberId);
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}
}
