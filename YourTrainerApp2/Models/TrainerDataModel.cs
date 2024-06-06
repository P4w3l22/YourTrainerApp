using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourTrainerApp.Models;

public class TrainerDataModel
{
	public int TrainerId { get; set; }
	public string TrainerName { get; set; }
	public string Description { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public float Rate { get; set; }
	public int Availability { get; set; }
}
