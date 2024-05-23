namespace YourTrainerApp2.Models;

public class TrainingPlanExercise
{
	public int Id { get; set; }
	public int? TpId { get; set; }
	public int EId { get; set; }
	public int Series { get; set; }
	public string? Reps { get; set; }
	public string? Weights { get; set; }
}
