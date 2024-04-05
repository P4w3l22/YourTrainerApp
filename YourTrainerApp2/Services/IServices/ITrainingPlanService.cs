﻿using YourTrainerApp2.Models;

namespace YourTrainerApp.Services.IServices
{
    public interface ITrainingPlanService
    {
        Task<T> CreateAsync<T>(TrainingPlan trainingPlan);
        Task<T> DeleteAsync<T>(int id);
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> UpdateAsync<T>(TrainingPlan trainingPlan);
    }
}