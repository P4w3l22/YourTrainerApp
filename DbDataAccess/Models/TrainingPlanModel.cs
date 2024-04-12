using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDataAccess.Models;

public class TrainingPlanModel
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string TrainingDays { get; set; }
	public string Notes { get; set; }
	public string Creator { get; set; }
	public List<ExerciseTrainingPlan>? Exercises { get; set; }
	public Dictionary<string, bool> TrainingDaysDict { get; set; }

	//public void CreateTrainingDaysDict()
	//{
	//	TrainingDaysDict = new();
	//	string[] splitedTrainingDaysDb = TrainingDays.Split(';');
	//	foreach (string day in splitedTrainingDaysDb)
	//	{
	//		List<string> dayKeyValue = day.Split(':').ToList();
	//		TrainingDaysDict.Add(dayKeyValue[0], dayKeyValue[1] == "0" ? false : true);
	//	}
	//}

}
