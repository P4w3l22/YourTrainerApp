using YourTrainer_App.Areas.Trainer.Models;
using YourTrainerApp.Areas.GymMember.Models;
using YourTrainerApp.Models;

namespace YourTrainer_App.Services.DataServices;

public interface ITrainerClientDataService
{
	//Task SendCooperationProposal(int trainerId, int memberId); //klient
	//Task<string> GetCooperationProposalResponse(int memberId); //klient
	//Task AcceptCooperationProposal(int trainerId, int memberId, int proposalId); //wspolne
	//Task RejectCooperationProposal(int trainerId, int memberId, int proposalId); //wspolne
	//Task DeleteTrainerClientCooperation(int memberId); //wspolne
	//Task<List<TrainerClientContact>> GetCooperationProposals(int trainerId); //wspolne
	//Task<List<CooperationProposal>> GetCooperationProposalsData(int trainerId); //wspolne
	Task<List<ClientContact>> GetClientsDetails(int trainerId); //trener
	Task<MemberDataModel> GetMemberData(int memberId); //klient
	Task<TrainerDataModel> GetTrainerData(int trainerId);
	Task<TrainerContact> GetTrainerDetails(int trainerId, int memberId); //klient
	Task<List<TrainerDataModel>> GetTrainersOptions(); //klient
	//Task SendMessage(string newMessage, int senderId, int receiverId); //wspolne
	//Task SendMessage(string newMessage, int senderId, int receiverId, string messageType);
}