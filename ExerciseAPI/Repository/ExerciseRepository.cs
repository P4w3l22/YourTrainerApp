using ExerciseAPI.Data;
using ExerciseAPI.Models;
using ExerciseAPI.Repository.IRepository;
using System.Linq.Expressions;

namespace ExerciseAPI.Repository
{
	public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
	{
		private readonly ApplicationDbContext _db;
        public ExerciseRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

		public async Task<Exercise> UpdateAsync(Exercise model)
		{
			_db.Exercises.Update(model);
			await _db.SaveChangesAsync();
			return model;
		}
	}
}
