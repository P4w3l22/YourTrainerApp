namespace YourTrainer_DBDataAccess.Models;

public class ExerciseTrainingPlan
{
	public int Id { get; set; }
	public ExerciseModel? ExerciseData { get; set; }
	public int? Series { get; set; }
	public string? Reps { get; set; }
	public string? Weights { get; set; }
}
