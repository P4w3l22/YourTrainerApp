using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
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
		APIResponse apiResponse = await _trainerClientContactService.GetCooperationProposals<APIResponse>(trainerId, MessageType.ConfirmClient.ToString());
		return JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));
	}

	public async Task<List<CooperationProposal>> GetCooperationProposalsData(int trainerId)
	{
		List<TrainerClientContact> trainerCooperationProposals = await GetCooperationProposals(trainerId);
		List<CooperationProposal> proposalsData = new();

		foreach (TrainerClientContact proposal in trainerCooperationProposals)
		{
			proposalsData.Add(new()
			{
				Message = proposal,
				ClientData = await GetMemberData(proposal.SenderId)
			});
		}

		return proposalsData;
	}

	public async Task SendCooperationProposal(int trainerId, int memberId)
	{
		MemberDataModel memberData = await GetMemberData(memberId);
		memberData.TrainersId = "-1";
		await _memberDataService.UpdateAsync<APIResponse>(memberData);

		await SendMessage("Klient wysyła Ci prośbę o współpracę", memberId, trainerId, MessageType.ConfirmClient.ToString());
	}

	public async Task<string> GetCooperationProposalResponse(int memberId)
	{
		APIResponse apiResponse = await _trainerClientContactService.GetCooperationProposals<APIResponse>(memberId, MessageType.AcceptClient.ToString());
		TrainerClientContact accepted = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result)).LastOrDefault();

		apiResponse = await _trainerClientContactService.GetCooperationProposals<APIResponse>(memberId, MessageType.RejectClient.ToString());
		TrainerClientContact rejected = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result)).LastOrDefault();

		if (accepted is not null)
		{
			await _trainerClientContactService.SetAsReadAsync<APIResponse>(accepted.Id);
			return "Accepted";
		}
		if (rejected is not null)
		{
			await _trainerClientContactService.SetAsReadAsync<APIResponse>(rejected.Id);
			return "Rejected";
		}
		return "NoResponse";
	}


	public async Task AcceptCooperationProposal(int trainerId, int memberId, int proposalId)
	{
		await _trainerClientContactService.SetAsReadAsync<APIResponse>(proposalId);
		await AddTrainerClientCooperation(trainerId, memberId);
		await SendMessage("Trener zaakceptował Twoją propozycję!", trainerId, memberId, MessageType.AcceptClient.ToString());
	}

	public async Task RejectCooperationProposal(int trainerId, int memberId, int proposalId)
	{
		MemberDataModel memberData = await GetMemberData(memberId);
		memberData.TrainersId = "";
		await _memberDataService.UpdateAsync<APIResponse>(memberData);

		await _trainerClientContactService.SetAsReadAsync<APIResponse>(proposalId);
		await SendMessage("Trener odmówił współpracy", trainerId, memberId, MessageType.RejectClient.ToString());
	}

	private async Task AddTrainerClientCooperation(int trainerId, int memberId)
	{
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

		if (memberData is not null && (memberData.TrainersId == "0" || memberData.TrainersId == "-1" || memberData.TrainersId.IsNullOrEmpty()))
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
		List<MemberDataModel> clients = new();

		if (trainerData.MembersId.Length > 0)
		{
			List<string> clientsId = trainerData.MembersId.Split(';').ToList();
			

			foreach (string clientId in clientsId)
			{
				clients.Add(await GetMemberData(int.Parse(clientId)));
			}
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
