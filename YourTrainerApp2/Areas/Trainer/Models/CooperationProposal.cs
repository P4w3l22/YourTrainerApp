using YourTrainerApp.Models;

namespace YourTrainer_App.Areas.Trainer.Models;

public class CooperationProposal
{
	public TrainerClientContact Message { get; set; }
	public MemberDataModel ClientData { get; set; }
}
