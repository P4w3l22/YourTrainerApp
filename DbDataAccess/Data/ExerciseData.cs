using DbDataAccess.DbAccess;
using DbDataAccess.Models;
using YourTrainer_DBDataAccess.Data.IData;

namespace DbDataAccess.Data;

public class ExerciseData : IExerciseData
{
	private readonly ISqlDataAccess _db;

	public ExerciseData(ISqlDataAccess db)
	{
		_db = db;
	}


	public async Task<IEnumerable<ExerciseModel>> GetExercises() =>
		await _db.GetData<ExerciseModel, dynamic>("spExercise_GetAll", new { });


	public async Task<ExerciseModel> GetExercise(int id)
	{
		var exercise = await _db.GetData<ExerciseModel, dynamic>("spExercise_Get", new { Id = id });
		return exercise.FirstOrDefault() ?? throw new InvalidOperationException("Nie znaleziono ćwiczenia w bazie danych o tym id");
	}


	public async Task InsertExercise(ExerciseModel exercise) =>
		await _db.SaveData("spExercise_Insert", new
		{
			exercise.Name,
			exercise.Force,
			exercise.Level,
			exercise.Mechanic,
			exercise.Equipment,
			exercise.PrimaryMuscles,
			exercise.SecondaryMuscles,
			exercise.Instructions,
			exercise.Category,
			exercise.ImgPath1,
			exercise.ImgPath2
		});


	public async Task UpdateExercise(ExerciseModel exercise) =>
		await _db.SaveData("spExercise_Update", exercise);


	public async Task DeleteExercise(int id) =>
		await _db.SaveData("spExercise_Delete", new { Id = id });
}
