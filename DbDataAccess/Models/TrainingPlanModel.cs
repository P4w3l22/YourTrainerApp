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
	
}
