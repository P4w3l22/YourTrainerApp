﻿namespace DbDataAccess.Models;

public class TrainerDataModel
{
	public int TrainerId { get; set; }
	public string? TrainerName { get; set; }
	public string? Description { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public decimal Rate { get; set; }
	public string? MembersId { get; set; }
	public int Availability { get; set; }
}
