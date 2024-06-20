using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
	private List<TrainerClientContact> _proposals
	{
		get => JsonConvert.DeserializeObject<List<TrainerClientContact>>(HttpContext.Session.GetString("Proposals") ?? JsonConvert.SerializeObject(new List<TrainerClientContact>()));
		set => HttpContext.Session.SetString("Proposals", JsonConvert.SerializeObject(value));
	}

	public ClientContactController(ITrainerClientDataService trainerClientDataService)
	{
		_trainerClientDataService = trainerClientDataService;
	}

	public async Task<IActionResult> Index()
	{
		List<TrainerClientContact> trainerCooperationProposals = await _trainerClientDataService.GetCooperationProposals(_trainerId);

		if (!trainerCooperationProposals.IsNullOrEmpty())
		{
			return RedirectToAction("ShowCooperationProposals", "ClientContact", new { Area = "Trainer" });
		}

		return RedirectToAction("ClientsDetails");
	}

	public async Task<IActionResult> ClientsDetails()
	{
		List<ClientContact> clientsContact = await _trainerClientDataService.GetClientsDetails(_trainerId);

        return View(clientsContact);
	}

	public async Task<List<CooperationProposal>> GetCooperationProposals()
	{
		_proposals = await _trainerClientDataService.GetCooperationProposals(_trainerId);;
		List<CooperationProposal> proposals = await _trainerClientDataService.GetCooperationProposalsData(_trainerId);
		return proposals;
	}

	public async Task<IActionResult> ShowCooperationProposals()
	{
		List<CooperationProposal> proposals = await GetCooperationProposals();

		return View(proposals);
	}

	public async Task<IActionResult> RejectCooperationProposal(int proposalIndex)
	{
		TrainerClientContact proposal = _proposals[proposalIndex];
		await _trainerClientDataService.RejectCooperationProposal(proposal.ReceiverId, proposal.SenderId, proposal.Id);
		return RedirectToAction("Index");
	}

	public async Task<IActionResult> AcceptCooperationProposal(int proposalIndex)
	{
		TrainerClientContact proposal = _proposals[proposalIndex];
		await _trainerClientDataService.AcceptCooperationProposal(proposal.ReceiverId, proposal.SenderId, proposal.Id);
		return RedirectToAction("Index");
	}

	public async Task<IActionResult> SendMessage(string newMessage, int memberId)
	{
		await _trainerClientDataService.SendMessage(newMessage, _trainerId, memberId);

		return RedirectToAction("Index");
	}
}
