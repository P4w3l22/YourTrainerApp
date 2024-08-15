using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using YourTrainer_App.Areas.Trainer.Models;
using YourTrainer_App.Areas.Trainer.Services;
using YourTrainer_App.Services.DataServices;
using YourTrainer_App.Models;
using static YourTrainer_Utility.StaticDetails;

namespace YourTrainer_App.Areas.Trainer.Controllers;

[Area("Trainer")]
public class ClientContactController : Controller
{
	private readonly ITrainerClientDataService _trainerClientDataService;
	private readonly ICooperationProposalService _cooperationProposalService;
	private readonly IMessagingService _messagingService;
	private readonly ITrainerDataSettingsService _dataSettingsService;

	private int _trainerId => int.Parse(HttpContext.Session.GetString("UserId"));
	private List<TrainerClientContact> _proposals
	{
		get => JsonConvert.DeserializeObject<List<TrainerClientContact>>(HttpContext.Session.GetString("Proposals") ?? JsonConvert.SerializeObject(new List<TrainerClientContact>()));
		set => HttpContext.Session.SetString("Proposals", JsonConvert.SerializeObject(value));
	}

	public ClientContactController(ITrainerClientDataService trainerClientDataService, ICooperationProposalService cooperationProposalService, IMessagingService messagingService, ITrainerDataSettingsService dataSettingsService)
	{
		_trainerClientDataService = trainerClientDataService;
		_cooperationProposalService = cooperationProposalService;
		_messagingService = messagingService;
		_dataSettingsService = dataSettingsService;
	}

	[Authorize(Roles = "trainer")]
	public async Task<IActionResult> Index()
	{
		List<TrainerClientContact> trainerCooperationProposals = await _cooperationProposalService.GetCooperationProposals(_trainerId);

		if (!trainerCooperationProposals.IsNullOrEmpty())
		{
			return RedirectToAction("ShowCooperationProposals", "ClientContact", new { Area = "Trainer" });
		}
		if (!await _dataSettingsService.TrainerDataIsPresent(_trainerId))
		{
			TempData["error"] = "Najpierw uzupełnij swoje dane";
			return RedirectToAction("ShowData", "DataSettings", new { Area = "Trainer" });
		}

		return RedirectToAction("ClientsDetails");
	}

	[Authorize(Roles = "trainer")]
	public async Task<IActionResult> ClientsDetails()
	{
		List<ClientContact> clientsContact = await _trainerClientDataService.GetClientsDetails(_trainerId);

        return View(clientsContact);
	}

	[Authorize(Roles = "trainer")]
	public async Task<List<CooperationProposal>> GetCooperationProposals()
	{
		_proposals = await _cooperationProposalService.GetCooperationProposals(_trainerId);;
		List<CooperationProposal> proposals = await _cooperationProposalService.GetCooperationProposalsData(_trainerId);
		return proposals;
	}

	[Authorize(Roles = "trainer")]
	public async Task<IActionResult> ShowCooperationProposals()
	{
		List<CooperationProposal> proposals = await GetCooperationProposals();

		return View(proposals);
	}

	[Authorize(Roles = "trainer")]
	public async Task<IActionResult> RejectCooperationProposal(int proposalIndex)
	{
		TrainerClientContact proposal = _proposals[proposalIndex];
		await _cooperationProposalService.RejectCooperationProposal(proposal.ReceiverId, proposal.SenderId, proposal.Id);
		return RedirectToAction("Index");
	}

	[Authorize(Roles = "trainer")]
	public async Task<IActionResult> AcceptCooperationProposal(int proposalIndex)
	{
		TrainerClientContact proposal = _proposals[proposalIndex];
		await _cooperationProposalService.AcceptCooperationProposal(proposal.ReceiverId, proposal.SenderId, proposal.Id);
		return RedirectToAction("Index");
	}

	[Authorize(Roles = "trainer")]
	public async Task<IActionResult> SendMessage(string newMessage, int memberId)
	{
		await _messagingService.SendMessage(newMessage, _trainerId, memberId, MessageType.Text.ToString());

		return RedirectToAction("Index");
	}

	[Authorize(Roles = "trainer")]
	public IActionResult SendTrainingPlanToClient(int clientId)
	{
		HttpContext.Session.SetString("SenderReceiverId", _trainerId.ToString() + ";" + clientId.ToString());
		return RedirectToAction("Upsert", "TrainingPlan", new { Area = "Visitor", isEditing = false });
	}
}
