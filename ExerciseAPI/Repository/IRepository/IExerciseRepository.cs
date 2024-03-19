using ExerciseAPI.Models;

namespace ExerciseAPI.Repository.IRepository
{
	public interface IExerciseRepository : IRepository<Exercise>
	{
		Task<Exercise> UpdateAsync(Exercise model);
	}
}
