using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourTrainerApp.Models;

public class MemberDataModel
{
	public int MemberId { get; set; }
	public string MemberName { get; set; }
	public string Description { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public string TrainersId { get; set; }
	public string TrainersPlan { get; set; }
}
