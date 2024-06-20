using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using YourTrainer_App.Areas.Trainer.Models;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_App.Services.DataServices;
using YourTrainer_Utility;
using YourTrainerApp.Models;

namespace YourTrainer_App.Areas.Trainer.Controllers;

[Area("Trainer")]
public class ClientContactController : Controller
{
	private readonly ITrainerClientDataService _trainerClientDataService;
	private int _trainerId => int.Parse(HttpContext.Session.GetString("UserId"));

	public ClientContactController(ITrainerClientDataService trainerClientDataService)
	{
		_trainerClientDataService = trainerClientDataService;
	}

	public async Task<IActionResult> Index()
	{
		List<TrainerClientContact> trainerCooperationProposals = await _trainerClientDataService.GetCooperationProposals(_trainerId);

		return RedirectToAction("ClientsDetails");
	}

	public async Task<IActionResult> ClientsDetails()
	{
		List<ClientContact> clientsContact = await _trainerClientDataService.GetClientsDetails(_trainerId);

        return View(clientsContact);
	}

	public async Task<IActionResult> SendMessage(string newMessage, int memberId)
	{
		await _trainerClientDataService.SendMessage(newMessage, _trainerId, memberId);

		return RedirectToAction("Index");
	}
}
