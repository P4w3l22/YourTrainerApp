using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourTrainer_App.Models;

public class MemberDataModel
{
	[Required]
	public int MemberId { get; set; }
	[Required(ErrorMessage = "Pole imienia jest wymagane")]
	public string? MemberName { get; set; }
	[Required(ErrorMessage = "Pole opisu jest wymagane")]
	public string? Description { get; set; }
	[Required(ErrorMessage = "Pole adresu email jest wymagane")]
	public string? Email { get; set; }
	[Required(ErrorMessage = "Pole numeru telefonu jest wymagane")]
	public string? PhoneNumber { get; set; }
	public string? TrainersId { get; set; }
	public string? TrainersPlan { get; set; }
}
