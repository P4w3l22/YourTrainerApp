using YourTrainer_App.Areas.Trainer.Models;
using YourTrainerApp.Models;

namespace YourTrainer_App.Repository.DataRepository;

public interface ITrainerClientDataRepository
{
	Task<List<ClientContact>> GetClientsDetails(int trainerId);

}