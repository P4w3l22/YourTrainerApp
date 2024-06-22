namespace ExerciseAPI.Models.DTO;

public class TrainingPlanExerciseUpdateDTO
{
	public int Id { get; set; }
	public int TPId { get; set; }
	public int EId { get; set; }
	public int Series { get; set; }
	public string? Reps { get; set; }
	public string? Weights { get; set; }
}
