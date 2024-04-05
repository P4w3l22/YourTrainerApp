//using YourTrainerApp2.Data;
using Microsoft.EntityFrameworkCore;
using YourTrainerApp2.Services.IServices;
using YourTrainerApp2.Services;
using YourTrainerApp.Services.IServices;
using YourTrainerApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

builder.Services.AddHttpClient<ITrainingPlanService, TrainingPlanService>();
builder.Services.AddScoped<ITrainingPlanService, TrainingPlanService>();

builder.Services.AddHttpClient<ITrainingPlanExerciseService, TrainingPlanExerciseService>();
builder.Services.AddScoped<ITrainingPlanExerciseService, TrainingPlanExerciseService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
