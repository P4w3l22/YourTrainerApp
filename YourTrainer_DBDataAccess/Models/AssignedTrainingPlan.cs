﻿namespace YourTrainer_DBDataAccess.Models;

public class AssignedTrainingPlan
{
	public int Id { get; set; }
	public int TrainerId { get; set; }
	public int ClientId { get; set; }
	public int PlanId { get; set; }
}
