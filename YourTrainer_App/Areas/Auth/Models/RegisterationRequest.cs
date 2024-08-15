using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace YourTrainer_App.Areas.Auth.Models;

public class RegisterationRequest
{
	[Required(ErrorMessage = "Pole nazwy użytkownika jest wymagane")]
	public string Username { get; set; }

	[Required(ErrorMessage = "Pole imienia jest wymagane")]
	public string Name { get; set; }

	[Required(ErrorMessage = "Podaj hasło")]
	public string Password { get; set; }

	[Required(ErrorMessage = "Pole roli jest wymagane")]
	public string Role { get; set; }

}