using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using Xunit.Abstractions;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_App.Services.DataServices;
using YourTrainerApp.Models;
using static YourTrainer_Utility.StaticDetails;
using Assert = Xunit.Assert;

namespace YourTrainer_Tests.App.Services;

public class TrainerClientDataServiceTests
{
	private Mock<ITrainerDataService> _trainerDataService;
	private Mock<IMemberDataService> _memberDataService;
	private Mock<ITrainerClientContactService> _trainerClientContactService;
	private Mock<IAssignedTrainingPlanService> _assignedTrainingPlanService;
	private Mock<ITrainingPlanService> _trainingPlanService;

	private Mock<ITrainerClientDataService> _mockTrainerClientDataService;

	private readonly ITestOutputHelper _output;

	public TrainerClientDataServiceTests(ITestOutputHelper output)
	{
		_trainerDataService = new();
		_memberDataService = new();
		_trainerClientContactService = new();
		_assignedTrainingPlanService = new();
		_trainingPlanService = new();

		_output = output;
	}

	//[Fact]
	//public async Task GetClientDetails_ReturnClientContactList()
	//{
	//	Mock<ITrainerClientContactService> mockTrainerClientContactService = new();
		
	//	MemberDataModel client2 = GetSampleMemberDataModel(2);
	//	MemberDataModel client3 = GetSampleMemberDataModel(3);

	//	TrainerClientContact messages1 = GetSampleMessagesWithClient(1);
	//	TrainerClientContact messages2 = GetSampleMessagesWithClient(2);

	//	TrainingPlan trainingPlan1 = GetSampleTrainingPlan(1);
	//	TrainingPlan trainingPlan2 = GetSampleTrainingPlan(2);

	//	int trainerId = 1;
		
	//	List<MemberDataModel> clients = new(){ client2, client3 };
	//	List<TrainerClientContact> messagesWithClients = new() { messages1, messages2 };
	//	List<TrainingPlan> trainingPlans = new() { trainingPlan1, trainingPlan2 };

	//	mockTrainerClientContactService.Setup(s => s.GetClients(trainerId)).ReturnsAsync(clients);
	//	mockTrainerClientContactService.Setup(s => s.GetSortedMessages(trainerId, 1)).ReturnsAsync(messagesClientOne);
	//	mockTrainerClientContactService.Setup(s => s.GetSortedMessages(trainerId, 2)).ReturnsAsync(messagesClientTwo);
	//	mockTrainerClientContactService.Setup(s => s.GetAssignedTrainingPlans(1)).ReturnsAsync(plansClientOne);
	//	mockTrainerClientContactService.Setup(s => s.GetAssignedTrainingPlans(2)).ReturnsAsync(plansClientTwo);
	//}

	[Fact]
	public async Task GetTrainersOption_ReturnTrainerList()
	{
		TrainerDataModel trainer1 = GetSampleTrainerDataModel(1);
		TrainerDataModel trainer2 = GetSampleTrainerDataModel(2);

		APIResponse apiResponse = new APIResponse();
		apiResponse.Result = JsonConvert.SerializeObject( new List<TrainerDataModel>() { trainer1, trainer2 });
		apiResponse.StatusCode = HttpStatusCode.OK;

		_trainerDataService.Setup(service => service.GetAllAsync<APIResponse>()).Returns(Task.FromResult(apiResponse));

		ITrainerClientDataService service = 
			new TrainerClientDataService(_trainerDataService.Object,
										 _memberDataService.Object,
										 _trainerClientContactService.Object,
										 _assignedTrainingPlanService.Object,
										 _trainingPlanService.Object);

		List<TrainerDataModel> trainers = await service.GetTrainersOptions();

		AssertTrainersModelAreEqual(trainers[0], trainer1);
		AssertTrainersModelAreEqual(trainers[1], trainer2);
	}

	[Fact]
	public async Task GetMemberData_ReturnMemberData()
	{
		MemberDataModel member1 = GetSampleMemberDataModel(1);
		
		APIResponse apiResponse = new APIResponse();
		apiResponse.Result = JsonConvert.SerializeObject(member1);
		apiResponse.StatusCode = HttpStatusCode.OK;

		_memberDataService.Setup(service => service.GetAsync<APIResponse>(member1.MemberId)).Returns(Task.FromResult(apiResponse));

		ITrainerClientDataService service =
			new TrainerClientDataService(_trainerDataService.Object,
										 _memberDataService.Object,
										 _trainerClientContactService.Object,
										 _assignedTrainingPlanService.Object,
										 _trainingPlanService.Object);

		MemberDataModel member = await service.GetMemberData(member1.MemberId);

		AssertMembersModelAreEqual(member, member1);
	}


	private TrainingPlan GetSampleTrainingPlan(int id)
	{
		TrainingPlan trainingPlan = new();
		trainingPlan.Id = id;
		trainingPlan.Title = "Test" + id.ToString();
		trainingPlan.CreateTrainingDaysString();
		trainingPlan.Notes = "Opis" + id.ToString();
		trainingPlan.Creator = "Trainer" + id.ToString();
		trainingPlan.Exercises = new()
		{
			GetSampleTrainingPlanExercise(id)
		};

		return trainingPlan;
	}

	private TrainingPlanExercise GetSampleTrainingPlanExercise(int id) =>
		new()
		{
			Id = 3*id,
			TpId = id,
			EId = 2*id,
			Series = 2,
			Reps = "5;5",
			Weights = "40;40"
		};

	private TrainerClientContact GetSampleMessagesWithClient(int id) =>
		new()
		{
			Id = id,
			SenderId = id + 1,
			ReceiverId = id + 2,
			MessageType = MessageType.Text.ToString(),
			MessageContent = "Test" + id.ToString(),
			IsRead = 0,
			SendDateTime = DateTime.Now
		};

	private TrainerDataModel GetSampleTrainerDataModel(int id) =>
		new()
		{
			TrainerId = id,
			TrainerName = "Trener" + id.ToString(),
			Description = "Opis trenera" + id.ToString(),
			Email = "trener" + id.ToString() + "@gmail.com",
			PhoneNumber = "0123456789",
			Rate = 5.0M,
			MembersId = "2;3",
			Availability = 1
		};

	private MemberDataModel GetSampleMemberDataModel(int id) =>
		new()
		{
			MemberId = id,
			MemberName = "Klient" + id.ToString(),
			Description = "Opis klienta" + id.ToString(),
			Email = "klient" + id.ToString() + "@gmail.com",
			PhoneNumber = "0123456789",
			TrainersId = "1",
			TrainersPlan = "4"
		};

	private void AssertMembersModelAreEqual(MemberDataModel member1, MemberDataModel member2)
	{
		Assert.Equal(member1.MemberId, member2.MemberId);
		Assert.Equal(member1.MemberName, member2.MemberName);
		Assert.Equal(member1.Description, member2.Description);
		Assert.Equal(member1.Email, member2.Email);
		Assert.Equal(member1.PhoneNumber, member2.PhoneNumber);
		Assert.Equal(member1.TrainersId, member2.TrainersId);
		Assert.Equal(member1.TrainersPlan, member2.TrainersPlan);
	}

	private void AssertTrainersModelAreEqual(TrainerDataModel trainer1, TrainerDataModel trainer2)
	{
		Assert.Equal(trainer1.TrainerId, trainer2.TrainerId);
		Assert.Equal(trainer1.TrainerName, trainer2.TrainerName);
		Assert.Equal(trainer1.Description, trainer2.Description);
		Assert.Equal(trainer1.Email, trainer2.Email);
		Assert.Equal(trainer1.PhoneNumber, trainer2.PhoneNumber);
		Assert.Equal(trainer1.Rate, trainer2.Rate);
		Assert.Equal(trainer1.MembersId, trainer2.MembersId);
		Assert.Equal(trainer1.Availability, trainer2.Availability);
	}
}
