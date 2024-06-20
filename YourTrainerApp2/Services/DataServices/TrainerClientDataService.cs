﻿using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using YourTrainer_App.Areas.Trainer.Models;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainerApp.Areas.GymMember.Models;
using YourTrainerApp.Models;
using static YourTrainer_Utility.StaticDetails;

namespace YourTrainer_App.Services.DataServices;

public class TrainerClientDataService : ITrainerClientDataService
{
	private readonly ITrainerDataService _trainerDataService;
	private readonly IMemberDataService _memberDataService;
	private readonly ITrainerClientContactService _trainerClientContactService;

	public TrainerClientDataService(ITrainerDataService trainerDataService, IMemberDataService memberDataService, ITrainerClientContactService trainerClientContactService)
	{
		_trainerDataService = trainerDataService;
		_memberDataService = memberDataService;
		_trainerClientContactService = trainerClientContactService;
	}


	public async Task<List<TrainerClientContact>> GetCooperationProposals(int trainerId)
	{
		APIResponse apiResponse = await _trainerClientContactService.GetCooperationProposals<APIResponse>(trainerId);
		return JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));
	}


	public async Task AddTrainerClientCooperation(int trainerId, int memberId)
	{
		await SendMessage("Współpraca", memberId, trainerId, MessageType.ConfirmClient.ToString());

		TrainerDataModel trainerData = await GetTrainerData(trainerId);

		if (trainerData.MembersId == "0" || trainerData.MembersId.IsNullOrEmpty())
		{
			trainerData.MembersId = memberId.ToString();
		}
		else
		{
			bool memberExists = Array.Exists(trainerData.MembersId.Split(";"), mId => mId == memberId.ToString());
			if (!memberExists)
			{
				trainerData.MembersId += ";" + memberId.ToString();
			}
		}

		await _trainerDataService.UpdateAsync<APIResponse>(trainerData);

		MemberDataModel memberData = await GetMemberData(memberId);

		if (memberData is not null && memberData.TrainersId == "0" || memberData.TrainersId.IsNullOrEmpty())
		{
			memberData.TrainersId = trainerId.ToString();
		}
		else
		{
			bool trainerExists = Array.Exists(memberData.TrainersId.Split(";"), trainerId => trainerId == trainerId.ToString());
			if (!trainerExists)
			{
				memberData.TrainersId += ";" + trainerId.ToString();
			}
		}

		await _memberDataService.UpdateAsync<APIResponse>(memberData);
	}



	public async Task DeleteTrainerClientCooperation(int memberId)
	{
		MemberDataModel memberData = await GetMemberData(memberId);

		int trainerId = int.Parse(memberData.TrainersId);
		memberData.TrainersId = "";

		await _memberDataService.UpdateAsync<APIResponse>(memberData);

		TrainerDataModel trainerData = await GetTrainerData(trainerId);

		List<string> membersIdTrainer = trainerData.MembersId.Split(";").ToList();
		membersIdTrainer.RemoveAll(id => id == memberId.ToString());
		trainerData.MembersId = String.Join(";", membersIdTrainer);

		await _trainerDataService.UpdateAsync<APIResponse>(trainerData);
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

	public async Task SendMessage(string newMessage, int senderId, int receiverId)
	{
		if (!string.IsNullOrEmpty(newMessage))
		{
			TrainerClientContact messageToSend = new()
			{
				Id = 0,
				SenderId = senderId,
				ReceiverId = receiverId,
				MessageType = MessageType.Text.ToString(),
				MessageContent = newMessage
			};
			await _trainerClientContactService.SendMessageAsync<APIResponse>(messageToSend);
		}
	}

	private async Task SendMessage(string newMessage, int senderId, int receiverId, string messageType)
	{
		if (!string.IsNullOrEmpty(newMessage))
		{
			TrainerClientContact messageToSend = new()
			{
				Id = 0,
				SenderId = senderId,
				ReceiverId = receiverId,
				MessageType = messageType,
				MessageContent = newMessage
			};
			await _trainerClientContactService.SendMessageAsync<APIResponse>(messageToSend);
		}
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

	public async Task<MemberDataModel> GetMemberData(int memberId)
	{
		APIResponse apiResponse = await _memberDataService.GetAsync<APIResponse>(memberId);
		return JsonConvert.DeserializeObject<MemberDataModel>(Convert.ToString(apiResponse.Result));
	}

	private async Task<TrainerDataModel> GetTrainerData(int trainerId)
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(trainerId);
		return JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));
	}

	public async Task<List<TrainerDataModel>> GetTrainersOptions()
	{
		APIResponse apiResponse = await _trainerDataService.GetAllAsync<APIResponse>();
		return JsonConvert.DeserializeObject<List<TrainerDataModel>>(Convert.ToString(apiResponse.Result));
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
