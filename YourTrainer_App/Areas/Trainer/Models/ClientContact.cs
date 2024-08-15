using YourTrainer_App.Models;

namespace YourTrainer_App.Areas.Trainer.Models;

public class ClientContact
{
	public MemberDataModel ClientData { get; set; }
	public List<TrainerClientContact> MessagesWithClient { get; set; }
	public List<TrainingPlan> PlansToClient { get; set; }

}
