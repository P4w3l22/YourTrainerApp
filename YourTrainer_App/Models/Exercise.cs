using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace YourTrainer_App.Models;

public class Exercise
{
	[Key]
	[Required]
	public int Id { get; set; }
	[Required(ErrorMessage = "Pole nazwy jest wymagane")]
	public string? Name { get; set; }
	[Required(ErrorMessage = "Pole typu ćwiczenia jest wymagane")]
	public string? Force { get; set; }
	[Required(ErrorMessage = "Pole poziomu trudności jest wymagane")]
	public string? Level { get; set; }
	[Required(ErrorMessage = "Pole mechaniki ćwiczenia jest wymagane")]
	public string? Mechanic { get; set; }
	[Required(ErrorMessage = "Pole wyposażenia jest wymagane")]
	public string? Equipment { get; set; }
	[Required(ErrorMessage = "Pole mięśni głównych jest wymagane")]
	public string? PrimaryMuscles { get; set; }
	//[Required(ErrorMessage = "Pole mięśni pomocniczych jest wymagane")]
	public string? SecondaryMuscles { get; set; }
	[Required(ErrorMessage = "Pole instrukcji ćwiczenia jest wymagane")]
	public string? Instructions { get; set; }
	[Required(ErrorMessage = "Pole kategorii jest wymagane")]
	public string? Category { get; set; }
	public string? ImgPath1 { get; set; }
	public string? ImgPath2 { get; set; }
}
