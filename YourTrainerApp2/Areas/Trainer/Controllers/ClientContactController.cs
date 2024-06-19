using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using YourTrainer_App.Areas.Trainer.Models;
using YourTrainer_App.Repository.DataRepository;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainerApp.Models;

namespace YourTrainer_App.Areas.Trainer.Controllers;

[Area("Trainer")]
public class ClientContactController : Controller
{
	private readonly ITrainerClientDataRepository _trainerClientDataRepository;
	private int _trainerId => int.Parse(HttpContext.Session.GetString("UserId"));

	public ClientContactController(ITrainerClientDataRepository trainerClientDataRepository)
	{
		_trainerClientDataRepository = trainerClientDataRepository;
	}

	public IActionResult Index()
	{
		return RedirectToAction("ClientsDetails");
	}

	public async Task<IActionResult> ClientsDetails()
	{
		List<ClientContact> clientsContact = await _trainerClientDataRepository.GetClientsDetails(_trainerId);

        return View(clientsContact);
	}

	public async Task<IActionResult> SendMessage(string newMessage, int memberId)
	{
		await _trainerClientDataRepository.SendMessage(newMessage, _trainerId, memberId);

		return RedirectToAction("Index");
	}
}
