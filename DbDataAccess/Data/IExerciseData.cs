using DbDataAccess.Models;

namespace DbDataAccess.Data
{
	public interface IExerciseData
	{
		Task DeleteExercise(int id);
		Task<ExerciseModel> GetExercise(int id);
		Task<IEnumerable<ExerciseModel>> GetExercises();
		Task InsertExercise(ExerciseModel exercise);
		Task UpdateExercise(ExerciseModel exercise);
	}
}