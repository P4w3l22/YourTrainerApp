namespace YourTrainerApp2.Models;

public class TrainingPlan
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string TrainingDays { get; set; }
    public string Notes { get; set; }
    public string Creator { get; set; }
    public Dictionary<string, bool> TrainingDaysDict { get; set; }
    public List<Exercise> Exercises { get; set; }
}
