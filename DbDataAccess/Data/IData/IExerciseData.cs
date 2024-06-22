using DbDataAccess.Models;

namespace YourTrainer_DBDataAccess.Data.IData
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