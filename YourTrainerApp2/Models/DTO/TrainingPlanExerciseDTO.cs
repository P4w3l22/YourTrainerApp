namespace YourTrainerApp.Models.DTO;

public class TrainingPlanExerciseDTO
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? ImgPath { get; set; }
	public int Series { get; set; }
	public string[]? Reps { get; set; }
	public string[] Weights { get; set; }

    public TrainingPlanExerciseDTO(string weights)
    {
		Weights = weights.Split(';');
    }
}
