namespace YourTrainerApp2.Models;

public class TrainingPlan
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string TrainingDays
	{
		set => CreateTrainingDaysDict(value);
	}
    public string Notes { get; set; }
    public string Creator { get; set; }

	public Dictionary<string, bool> TrainingDaysDict {  get; set; }
    public List<Exercise> Exercises { get; set; }


    private void CreateTrainingDaysDict(string trainingDays)
    {
		TrainingDaysDict = new();
		string[] splitedTrainingDaysDb = trainingDays.Split(';');
		foreach (string day in splitedTrainingDaysDb)
		{
			List<string> dayKeyValue = day.Split(':').ToList();
			TrainingDaysDict.Add(dayKeyValue[0], dayKeyValue[1] == "0" ? false : true);
		}
	}

	private void CreateExercises()
	{

	}
}
