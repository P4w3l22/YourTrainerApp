using Microsoft.EntityFrameworkCore;
using YourTrainerApp.Services.IServices;
using YourTrainerApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpClient<ITrainingPlanService, TrainingPlanService>();
builder.Services.AddScoped<ITrainingPlanService, TrainingPlanService>();

builder.Services.AddHttpClient<ITrainingPlanExerciseService, TrainingPlanExerciseService>();
builder.Services.AddScoped<ITrainingPlanExerciseService, TrainingPlanExerciseService>();


builder.Services.AddSingleton<IHttpContextAccessor,  HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
})
.AddAuthentication(options =>
{
    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LogoutPath = "/Auth/Logout"; // Œcie¿ka wylogowania
}); ;

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
app.UseAuthentication();

app.UseSession();

app.MapControllerRoute(
        name: "default",
        pattern: "{area=Visitor}/{controller=Home}/{action=Index}/{id?}");

app.Run();
