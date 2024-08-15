using YourTrainer_App.Models;

namespace YourTrainer_App.Models;

public class TrainingPlan
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string TrainingDays { get; set; }
    public string Notes { get; set; }
    public string Creator { get; set; }

	public Dictionary<string, bool> TrainingDaysDict {  get; set; }
	public List<TrainingPlanExercise> Exercises { get; set; }

    public TrainingPlan()
    {
        TrainingDaysDict = new()
        {
            { "Poniedziałek", false },
            { "Wtorek", false },
            { "Środa", false },
            { "Czwartek", false },
            { "Piątek", false },
            { "Sobota", false },
            { "Niedziela", false }
        };
    }

    public void CreateTrainingDaysDict()
    {
		TrainingDaysDict = new();
		string[] splitedTrainingDaysDb = TrainingDays.Split(';');
		foreach (string day in splitedTrainingDaysDb)
		{
			List<string> dayKeyValue = day.Split(':').ToList();
			TrainingDaysDict.Add(dayKeyValue[0], dayKeyValue[1] == "0" ? false : true);
		}
	}

    public void CreateTrainingDaysString()
    {
        TrainingDays = string.Empty;

        foreach (KeyValuePair<string, bool> trainingDay in TrainingDaysDict)
        {
            TrainingDays += $"{trainingDay.Key}:" + (trainingDay.Value ? "1" : "0") + ";";
		}

        TrainingDays = TrainingDays.Substring(0, TrainingDays.Length - 1);
        Console.WriteLine(TrainingDays);
    }

}
