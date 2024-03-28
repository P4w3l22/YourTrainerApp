using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDataAccess.Models;

public class TrainingPlanWithExercisesModel
{
	public TrainingPlanModel Plan { get; set; }
	public List<TrainingPlanExerciseModel> PlanExercises { get; set; }
	
}
