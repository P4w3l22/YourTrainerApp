using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainer_App.Areas.Trainer.Models;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainerApp.Areas.GymMember.Models;
using YourTrainerApp.Models;

namespace YourTrainer_App.Repository.DataRepository;

public class TrainerClientDataRepository : ITrainerClientDataRepository
{
	private readonly ITrainerDataService _trainerDataService;
	private readonly IMemberDataService _memberDataService;
	private readonly ITrainerClientContactService _trainerClientContactService;

	public TrainerClientDataRepository(ITrainerDataService trainerDataService, IMemberDataService memberDataService, ITrainerClientContactService trainerClientContactService)
	{
		_trainerDataService = trainerDataService;
		_memberDataService = memberDataService;
		_trainerClientContactService = trainerClientContactService;
	}

	public async Task<TrainerContact> GetTrainerDetails(int trainerId, int memberId)
	{
		TrainerContact trainerContact = new();
		trainerContact.TrainerData = await GetTrainerData(trainerId);
		trainerContact.MessagesWithTrainer = await GetSortedMessages(trainerId, memberId);

		return trainerContact;
	}

	public async Task<List<ClientContact>> GetClientsDetails(int trainerId)
	{
		List<ClientContact> clientsContact = new();

		List<MemberDataModel> clients = await GetClients(trainerId);

		foreach (var client in clients)
		{
			clientsContact.Add(new()
			{
				ClientData = client,
				MessagesWithClient = await GetSortedMessages(trainerId, client.MemberId),
				PlansToClient = new()
			});
		}

		return clientsContact;
	}

	private async Task<List<MemberDataModel>> GetClients(int trainerId)
	{
		TrainerDataModel trainerData = await GetTrainerData(trainerId);

		List<string> clientsId = trainerData.MembersId.Split(';').ToList();
		List<MemberDataModel> clients = new();

		foreach (string clientId in clientsId)
		{
			clients.Add(await GetMemberData(int.Parse(clientId)));
		}

		return clients;
	}

	private async Task<MemberDataModel> GetMemberData(int memberId)
	{
		APIResponse apiResponse = await _memberDataService.GetAsync<APIResponse>(memberId);
		return JsonConvert.DeserializeObject<MemberDataModel>(Convert.ToString(apiResponse.Result));
	}

	private async Task<TrainerDataModel> GetTrainerData(int trainerId)
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(trainerId);
		return JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));
	}
	private async Task<List<TrainerClientContact>> GetSortedMessages(int trainerId, int memberId)
	{
		APIResponse apiResponse = await _trainerClientContactService.GetMessagesAsync<APIResponse>(trainerId, memberId, StaticDetails.MessageType.Text.ToString());
		List<TrainerClientContact> trainerMessages = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));

		apiResponse = await _trainerClientContactService.GetMessagesAsync<APIResponse>(memberId, trainerId, StaticDetails.MessageType.Text.ToString());
		List<TrainerClientContact> memberMessages = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));

		return trainerMessages.Concat(memberMessages).OrderBy(m => m.SendDateTime).ToList();
	}
}
