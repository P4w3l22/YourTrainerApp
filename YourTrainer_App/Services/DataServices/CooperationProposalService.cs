using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using YourTrainer_App.Areas.Trainer.Models;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_App.Models;
using static YourTrainer_Utility.StaticDetails;

namespace YourTrainer_App.Services.DataServices;

public class CooperationProposalService : ICooperationProposalService
{
	private readonly ITrainerClientContactService _trainerClientContactService;
	private readonly ITrainerClientDataService _trainerClientDataService;
	private readonly IMemberDataService _memberDataService;
	private readonly ITrainerDataService _trainerDataService;
	private readonly IMessagingService _messagingService;


	public CooperationProposalService(ITrainerClientContactService trainerClientContactService, ITrainerClientDataService trainerClientDataService, IMemberDataService memberDataService, ITrainerDataService trainerDataService, IMessagingService messagingService)
	{
		_trainerClientContactService = trainerClientContactService;
		_trainerClientDataService = trainerClientDataService;
		_memberDataService = memberDataService;
		_trainerDataService = trainerDataService;
		_messagingService = messagingService;
	}

	public async Task<List<TrainerClientContact>> GetCooperationProposals(int trainerId)
	{
		APIResponse apiResponse = await _trainerClientContactService.GetCooperationProposals<APIResponse>(trainerId, MessageType.ConfirmClient.ToString());
		List<TrainerClientContact>? trainerClientContacts = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));
		return trainerClientContacts ?? throw new InvalidOperationException("Nie otrzymano odpowiedzi z API w GetCooperationProposals");
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
				ClientData = await _trainerClientDataService.GetMemberData(proposal.SenderId)
			});
		}

		return proposalsData;
	}

	public async Task SendCooperationProposal(int trainerId, int memberId)
	{
		MemberDataModel memberData = await _trainerClientDataService.GetMemberData(memberId);
		memberData.TrainersId = "-1";
		await _memberDataService.UpdateAsync<APIResponse>(memberData);

		await _messagingService.SendMessage("Klient wysyła Ci prośbę o współpracę", memberId, trainerId, MessageType.ConfirmClient.ToString());
	}

	public async Task<string> GetCooperationProposalResponse(int memberId)
	{
		APIResponse apiResponse = await _trainerClientContactService.GetCooperationProposals<APIResponse>(memberId, MessageType.AcceptClient.ToString());
		TrainerClientContact? accepted = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result)).LastOrDefault();

		apiResponse = await _trainerClientContactService.GetCooperationProposals<APIResponse>(memberId, MessageType.RejectClient.ToString());
		TrainerClientContact? rejected = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result)).LastOrDefault();

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
		await _messagingService.SendMessage("Trener zaakceptował Twoją propozycję!", trainerId, memberId, MessageType.AcceptClient.ToString());
	}

	public async Task RejectCooperationProposal(int trainerId, int memberId, int proposalId)
	{
		MemberDataModel memberData = await _trainerClientDataService.GetMemberData(memberId);
		memberData.TrainersId = "";
		await _memberDataService.UpdateAsync<APIResponse>(memberData);

		await _trainerClientContactService.SetAsReadAsync<APIResponse>(proposalId);
		await _messagingService.SendMessage("Trener odmówił współpracy", trainerId, memberId, MessageType.RejectClient.ToString());
	}

	private async Task AddTrainerClientCooperation(int trainerId, int memberId)
	{
		TrainerDataModel trainerData = await _trainerClientDataService.GetTrainerData(trainerId);

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

		MemberDataModel memberData = await _trainerClientDataService.GetMemberData(memberId);

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
		MemberDataModel memberData = await _trainerClientDataService.GetMemberData(memberId);

		if (!memberData.TrainersId.IsNullOrEmpty() && memberData.TrainersId != "0")
		{
			int trainerId = int.Parse(memberData.TrainersId);
			memberData.TrainersId = "";

			await _memberDataService.UpdateAsync<APIResponse>(memberData);

			TrainerDataModel trainerData = await _trainerClientDataService.GetTrainerData(trainerId);

			List<string> membersIdTrainer = trainerData.MembersId.Split(";").ToList();
			membersIdTrainer.RemoveAll(id => id == memberId.ToString());
			trainerData.MembersId = String.Join(";", membersIdTrainer);

			await _trainerDataService.UpdateAsync<APIResponse>(trainerData);
		}
	}
}
