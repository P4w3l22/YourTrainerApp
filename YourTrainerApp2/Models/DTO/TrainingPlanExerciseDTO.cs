﻿namespace YourTrainerApp2.Models.DTO;

public class TrainingPlanExerciseDTO
{
	public int Id { get; set; }
	public int TPId { get; set; }
	public int EId { get; set; }
	public int Series { get; set; }
	public string Weights { get; set; }
}
