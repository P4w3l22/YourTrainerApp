namespace YourTrainerApp2.Models;

public class TrainingPlan
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<string> Days { get; set; }
    public string Notes { get; set; }

}
