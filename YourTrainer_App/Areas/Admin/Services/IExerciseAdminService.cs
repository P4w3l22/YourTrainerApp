using YourTrainer_App.Models;
using YourTrainer_App.Models.VM;

namespace YourTrainer_App.Areas.Admin.Services
{
	public interface IExerciseAdminService
	{
		Task<(string, string)> CreateExerciseAndGetResponse(ExerciseCreateVM exerciseCreated, string sessionToken);
		Task<(string, string)> DeleteExerciseAndGetResponse(int id, string sessionToken);
		Task<Exercise> GetExercise(int id);
		Task<List<Exercise>> GetExercisesList();
		Task<(string, string)> UpdateExerciseAndGetResponse(ExerciseCreateVM exerciseUpdated, string sessionToken);
	}
}