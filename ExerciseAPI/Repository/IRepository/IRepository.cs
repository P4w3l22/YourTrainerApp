﻿using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ExerciseAPI.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
		Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
		Task CreateAsync(T model);
		Task RemoveAsync(T model);
		Task SaveAsync();
	}
}
