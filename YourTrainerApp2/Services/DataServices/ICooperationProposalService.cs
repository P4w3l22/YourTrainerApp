using YourTrainer_App.Areas.Trainer.Models;
using YourTrainerApp.Models;

namespace YourTrainer_App.Services.DataServices
{
	public interface ICooperationProposalService
	{
		Task AcceptCooperationProposal(int trainerId, int memberId, int proposalId);
		Task DeleteTrainerClientCooperation(int memberId);
		Task<string> GetCooperationProposalResponse(int memberId);
		Task<List<TrainerClientContact>> GetCooperationProposals(int trainerId);
		Task<List<CooperationProposal>> GetCooperationProposalsData(int trainerId);
		Task RejectCooperationProposal(int trainerId, int memberId, int proposalId);
		Task SendCooperationProposal(int trainerId, int memberId);
	}
}