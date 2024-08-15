using Microsoft.IdentityModel.Tokens;
using MoreLinq;
using Newtonsoft.Json;
using YourTrainer_App.Areas.Trainer.Models;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainer_App.Areas.GymMember.Models;
using YourTrainer_App.Models;

namespace YourTrainer_App.Services.DataServices;

public class TrainerClientDataService : ITrainerClientDataService
{
	private readonly ITrainerDataService _trainerDataService;
	private readonly IMemberDataService _memberDataService;
	private readonly ITrainerClientContactService _trainerClientContactService;
	private readonly IAssignedTrainingPlanService _assignedTrainingPlanService;
	private readonly ITrainingPlanService _trainingPlanService;

	public TrainerClientDataService(ITrainerDataService trainerDataService, IMemberDataService memberDataService, ITrainerClientContactService trainerClientContactService, IAssignedTrainingPlanService assignedTrainingPlanService, ITrainingPlanService trainingPlanService)
	{
		_trainerDataService = trainerDataService;
		_memberDataService = memberDataService;
		_trainerClientContactService = trainerClientContactService;
		_assignedTrainingPlanService = assignedTrainingPlanService;
		_trainingPlanService = trainingPlanService;
	}


	public async Task<TrainerContact> GetTrainerDetails(int trainerId, int memberId)
	{
		TrainerContact trainerContact = new();
		trainerContact.TrainerData = await GetTrainerData(trainerId);
		trainerContact.MessagesWithTrainer = await GetSortedMessages(trainerId, memberId);
		trainerContact.PlansFromTrainer = await GetAssignedTrainingPlans(memberId);

		return trainerContact;
	}

	//private
	public async Task<List<TrainingPlan>> GetAssignedTrainingPlans(int clientId)
	{
		List<TrainingPlan> assignedTrainingPlans = new();
		APIResponse apiResponse = await _assignedTrainingPlanService.GetAsync<APIResponse>(clientId);

		AssignedTrainingPlan trainingPlanFromTrainer = new();
		if (apiResponse.Result is not null)
		{
			trainingPlanFromTrainer = JsonConvert.DeserializeObject<AssignedTrainingPlan>(apiResponse.Result.ToString());
			apiResponse = await _trainingPlanService.GetAsync<APIResponse>(trainingPlanFromTrainer.PlanId);
			assignedTrainingPlans.Add(JsonConvert.DeserializeObject<TrainingPlan>(apiResponse.Result.ToString()));
		}
	
		return assignedTrainingPlans;
	}

	public async Task<List<ClientContact>> GetClientsDetails(int trainerId)
	{
		List<ClientContact> clientsContact = new();

		List<MemberDataModel> clients = await GetClients(trainerId);

		if (clients.IsNullOrEmpty())
		{
			return new List<ClientContact>();
		}

		foreach (var client in clients)
		{
			clientsContact.Add(new()
			{
				ClientData = client,
				MessagesWithClient = await GetSortedMessages(trainerId, client.MemberId),
				PlansToClient = await GetAssignedTrainingPlans(client.MemberId)
			});
		}

		return clientsContact;
	}

	//private
	public async Task<List<MemberDataModel>> GetClients(int trainerId)
	{
		TrainerDataModel trainerData = await GetTrainerData(trainerId);
		List<MemberDataModel> clients = new();

		if (trainerData.MembersId.Length > 0 && trainerData.MembersId != "0")
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

	public async Task<TrainerDataModel> GetTrainerData(int trainerId)
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(trainerId);
		return JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));
	}

	public async Task<List<TrainerDataModel>> GetTrainersOptions()
	{
		APIResponse apiResponse = await _trainerDataService.GetAllAsync<APIResponse>();
		List<TrainerDataModel>? trainerOptions = JsonConvert.DeserializeObject<List<TrainerDataModel>>(Convert.ToString(apiResponse.Result));
		if (trainerOptions is not null)
		{
			return trainerOptions.Where(trainer => trainer.Availability == 1).ToList();
		}
		return new();
	}

	public async Task<List<TrainerClientContact>> GetSortedMessages(int trainerId, int memberId)
	{
		APIResponse? apiResponse = await _trainerClientContactService.GetMessagesAsync<APIResponse>(trainerId, memberId, StaticDetails.MessageType.Text.ToString());
		List<TrainerClientContact>? trainerMessages = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));

		apiResponse = await _trainerClientContactService.GetMessagesAsync<APIResponse>(memberId, trainerId, StaticDetails.MessageType.Text.ToString());
		List<TrainerClientContact>? memberMessages = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));

		return trainerMessages.Concat(memberMessages).OrderBy(m => m.SendDateTime).ToList();
	}

	public bool TrainerIsAssigned(MemberDataModel memberData) =>
		memberData is not null && !memberData.TrainersId.IsNullOrEmpty() &&
		memberData.TrainersId != "0" && memberData.TrainersId != "-1";
}
