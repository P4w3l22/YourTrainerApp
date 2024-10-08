﻿using YourTrainer_DBDataAccess.DbAccess;
using YourTrainer_DBDataAccess.Models;
using YourTrainer_DBDataAccess.Data.IData;

namespace YourTrainer_DBDataAccess.Data;

public class TrainerData : ITrainerData
{
	private readonly ISqlDataAccess _db;

	public TrainerData(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task<IEnumerable<TrainerDataModel>> GetTrainers() =>
		await _db.GetData<TrainerDataModel, dynamic>("spTrainerData_GetAll", new { });

	public async Task<TrainerDataModel> GetTrainer(int id)
	{
		var trainers = await _db.GetData<TrainerDataModel, dynamic>("spTrainerData_Get", new { TrainerId = id });
		return trainers.FirstOrDefault();
	}

	public async Task InsertTrainerData(TrainerDataModel trainerData) =>
		await _db.SaveData("spTrainerData_Insert", new
		{
			trainerData.TrainerId,
			trainerData.TrainerName,
			trainerData.Description,
			trainerData.Email,
			trainerData.PhoneNumber,
			trainerData.Rate,
			trainerData.MembersId,
			trainerData.Availability
		});

	public async Task UpdateTrainerData(TrainerDataModel trainerData) =>
		await _db.SaveData("spTrainerData_Update", new
		{
			trainerData.TrainerId,
			trainerData.TrainerName,
			trainerData.Description,
			trainerData.Email,
			trainerData.PhoneNumber,
			trainerData.Rate,
			trainerData.MembersId,
			trainerData.Availability
		});

	public async Task DeleteTrainerData(int id) =>
		await _db.SaveData("spTrainerData_Delete", new { TrainerId = id });
}