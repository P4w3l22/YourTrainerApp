using Microsoft.EntityFrameworkCore;
using YourTrainerApp2.Services.IServices;
using YourTrainerApp2.Services;
using YourTrainerApp.Services.IServices;
using YourTrainerApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

builder.Services.AddHttpClient<ITrainingPlanService, TrainingPlanService>();
builder.Services.AddScoped<ITrainingPlanService, TrainingPlanService>();

builder.Services.AddHttpClient<ITrainingPlanExerciseService, TrainingPlanExerciseService>();
builder.Services.AddScoped<ITrainingPlanExerciseService, TrainingPlanExerciseService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");

//    endpoints.MapControllerRoute(
//        name: "Admin",
//        pattern: "{area:exists}/{controller=ExerciseAdmin}/{action=Index}/{id?}");
//});



//app.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
        name: "default",
        pattern: "{area=Visitor}/{controller=Home}/{action=Index}/{id?}");

app.Run();
