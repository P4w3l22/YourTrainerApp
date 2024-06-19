using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using YourTrainer_App.Areas.Trainer.Models;
using YourTrainer_Utility;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;

namespace YourTrainer_App.Areas.Trainer.Controllers;

[Area("Trainer")]
public class ClientContactController : Controller
{
	private readonly ITrainerDataService _trainerDataService;
	private readonly IMemberDataService _memberDataService;
	private readonly ITrainerClientContactService _trainerClientContactService;
	private int _trainerId
	{
		get => int.Parse(HttpContext.Session.GetString("UserId"));
	}
	private int _memberId;

	public ClientContactController(ITrainerDataService trainerDataService, IMemberDataService memberDataService, ITrainerClientContactService trainerClientContactService)
	{
		_trainerDataService = trainerDataService;
		_memberDataService = memberDataService;
		_trainerClientContactService = trainerClientContactService;
	}


	public IActionResult Index()
	{
		return RedirectToAction("ClientsDetails");
	}

	public async Task<IActionResult> ClientsDetails()
	{
		List<ClientContact> clientsContact = new();

		List<MemberDataModel> clients = await GetClients();

        foreach (var client in clients)
        {
			clientsContact.Add(new()
			{
				ClientData = client,
				MessagesWithClient = await GetMessagesWithClient(client.MemberId),
				PlansToClient = new()
			});
        }

        return View(clientsContact);
	}

	private async Task<List<MemberDataModel>> GetClients()
	{
		TrainerDataModel trainerData = await GetTrainerData();

		List<string> clientsId = trainerData.MembersId.Split(';').ToList();
		List<MemberDataModel> clients = new();

		foreach (string clientId in clientsId)
		{
			clients.Add(await GetMemberData(int.Parse(clientId)));
		}

		return clients;
	}

	private async Task<TrainerDataModel> GetTrainerData()
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(_trainerId);
		return JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));
	}

	private async Task<MemberDataModel> GetMemberData(int id)
	{
		APIResponse apiResponse = await _memberDataService.GetAsync<APIResponse>(id);
		return JsonConvert.DeserializeObject<MemberDataModel>(Convert.ToString(apiResponse.Result));
	}

	private async Task<List<TrainerClientContact>> GetMessagesWithClient(int client)
	{
		APIResponse apiResponse = await _trainerClientContactService.GetMessagesAsync<APIResponse>(client, _trainerId, StaticDetails.MessageType.Text.ToString());
		List<TrainerClientContact> memberMessages = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));

		apiResponse = await _trainerClientContactService.GetMessagesAsync<APIResponse>(_trainerId, client, StaticDetails.MessageType.Text.ToString());
		List<TrainerClientContact> trainerMessages = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));

		return memberMessages.Concat(trainerMessages).OrderBy(m => m.SendDateTime).ToList();
	}
}
