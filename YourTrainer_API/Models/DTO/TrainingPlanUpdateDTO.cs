namespace YourTrainer_API.Models.DTO;

public class TrainingPlanUpdateDTO
{
	public int Id { get; set; }
	public string? Title { get; set; }
	public string? TrainingDays { get; set; }
	public string? Notes { get; set; }
	public string? Creator { get; set; }
}
