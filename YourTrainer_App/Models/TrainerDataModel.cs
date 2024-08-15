using System.ComponentModel.DataAnnotations;

namespace YourTrainer_App.Models;

public class TrainerDataModel
{
	[Required]
	public int TrainerId { get; set; }
	[Required(ErrorMessage = "Pole imienia jest wymagane")]
	public string? TrainerName { get; set; }
	[Required(ErrorMessage = "Pole opisu jest wymagane")]
	public string? Description { get; set; }
	[Required(ErrorMessage = "Pole adresu email jest wymagane")]
	public string? Email { get; set; }
	[Required(ErrorMessage = "Pole numeru telefonu jest wymagane")]
	public string? PhoneNumber { get; set; }
	public decimal Rate { get; set; }
	public string? MembersId { get; set; }
	[Required(ErrorMessage = "Pole dostępności jest wymagane")]
	public int Availability { get; set; }
}
