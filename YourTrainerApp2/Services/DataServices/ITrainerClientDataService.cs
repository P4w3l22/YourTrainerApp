using YourTrainer_App.Areas.Trainer.Models;
using YourTrainerApp.Areas.GymMember.Models;
using YourTrainerApp.Models;

namespace YourTrainer_App.Services.DataServices
{
	public interface ITrainerClientDataService
	{
		Task SendCooperationProposal(int trainerId, int memberId);
		Task<string> GetCooperationProposalResponse(int memberId);
		Task AcceptCooperationProposal(int trainerId, int memberId, int proposalId);
		Task RejectCooperationProposal(int trainerId, int memberId, int proposalId);
		Task DeleteTrainerClientCooperation(int memberId);
		Task<List<TrainerClientContact>> GetCooperationProposals(int trainerId);
		Task<List<CooperationProposal>> GetCooperationProposalsData(int trainerId);
		Task<List<ClientContact>> GetClientsDetails(int trainerId);
		Task<MemberDataModel> GetMemberData(int memberId);
		Task<TrainerContact> GetTrainerDetails(int trainerId, int memberId);
		Task<List<TrainerDataModel>> GetTrainersOptions();
		Task SendMessage(string newMessage, int senderId, int receiverId);
	}
}