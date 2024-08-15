namespace YourTrainer_App.Models.DTO;

public class TrainingPlanExerciseCreateDTO
{
	public int TPId { get; set; }
	public int EId { get; set; }
	public int Series { get; set; }
	public string Reps { get; set; }
	public string Weights { get; set; }
}
