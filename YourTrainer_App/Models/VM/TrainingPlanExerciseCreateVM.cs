namespace YourTrainer_App.Models.VM;

public class TrainingPlanExerciseCreateVM
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string? Equipment { get; set; }
	public string ImgPath1 { get; set; }
	public string ImgPath2 { get; set; }
}
