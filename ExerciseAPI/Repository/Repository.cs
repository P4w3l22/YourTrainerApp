using ExerciseAPI.Data;
using ExerciseAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExerciseAPI.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
			this.dbSet = _db.Set<T>();

        }

        public async Task CreateAsync(T model)
		{
			await dbSet.AddAsync(model);
			await SaveAsync();
		}

		public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			return await query.ToListAsync();
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.FirstOrDefaultAsync();
		}

		public async Task RemoveAsync(T model)
		{
			dbSet.Remove(model);
			await SaveAsync();
		}

		public async Task SaveAsync()
		{
			await _db.SaveChangesAsync();
		}
	}
}
