using DbDataAccess.DbAccess;
using DbDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDataAccess.Data;

public class TrainerData
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
		var exercise = await _db.GetData<TrainerDataModel, dynamic>("spTrainerData_Get", new { TrainerId = id });
		return exercise.FirstOrDefault();
	}

	public async Task InsertExercise(TrainerDataModel trainerData) =>
		await _db.SaveData("spTrainerData_Insert", new
		{
			trainerData.TrainerId,
			trainerData.Description,
			trainerData.Email,
			trainerData.PhoneNumber,
			trainerData.Availability
		});

	public async Task UpdateExercise(TrainerDataModel trainerData) =>
		await _db.SaveData("spTrainerData_Update", new
		{
			trainerData.TrainerId,
			trainerData.Description,
			trainerData.Email,
			trainerData.PhoneNumber,
			trainerData.Availability
		});

	public async Task DeleteExercise(int id) =>
		await _db.SaveData("spTrainerData_Delete", new { TrainerId = id });
}