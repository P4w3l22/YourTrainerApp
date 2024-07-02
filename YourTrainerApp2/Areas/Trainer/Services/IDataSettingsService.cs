
namespace YourTrainer_App.Areas.Trainer.Services
{
	public interface IDataSettingsService
	{
		Task<bool> TrainerDataIsPresent(int trainerId);
	}
}