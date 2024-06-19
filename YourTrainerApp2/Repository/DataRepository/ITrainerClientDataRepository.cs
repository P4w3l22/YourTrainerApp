using YourTrainer_App.Areas.Trainer.Models;
using YourTrainerApp.Areas.GymMember.Models;
using YourTrainerApp.Models;

namespace YourTrainer_App.Repository.DataRepository;

public interface ITrainerClientDataRepository
{
	Task<List<ClientContact>> GetClientsDetails(int trainerId);
	Task<TrainerContact> GetTrainerDetails(int trainerId, int memberId);
	Task AddTrainerClientCooperation(int trainerId, int memberId);
	Task DeleteTrainerClientCooperation(int memberId);
	Task SendMessage(string newMessage, int trainerId, int memberId);
	Task<MemberDataModel> GetMemberData(int memberId);
	Task<List<TrainerDataModel>> GetTrainersOptions();
}