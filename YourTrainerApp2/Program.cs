using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using YourTrainer_App.Services.APIServices;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_App.Services.DataServices;
using YourTrainer_App.Areas.Visitor.Services;
using YourTrainer_App.Areas.Admin.Services;

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

builder.Services.AddHttpClient<ITrainerDataService, TrainerDataService>();
builder.Services.AddScoped<ITrainerDataService, TrainerDataService>();

builder.Services.AddHttpClient<IMemberDataService, MemberDataService>();
builder.Services.AddScoped<IMemberDataService, MemberDataService>();

builder.Services.AddHttpClient<ITrainerClientContactService, TrainerClientContactService>();
builder.Services.AddScoped<ITrainerClientContactService, TrainerClientContactService>();

builder.Services.AddHttpClient<IAssignedTrainingPlanService, AssignedTrainingPlanService>();
builder.Services.AddScoped<IAssignedTrainingPlanService, AssignedTrainingPlanService>();


builder.Services.AddScoped<IExerciseAdminService, ExerciseAdminService>();
builder.Services.AddScoped<ITrainerClientDataService, TrainerClientDataService>();
builder.Services.AddScoped<ICooperationProposalService, CooperationProposalService>();
builder.Services.AddScoped<IMessagingService, MessagingService>();

builder.Services.AddScoped<ITrainingPlanDataService, TrainingPlanDataService>();
//builder.Services.AddScoped<ITrainingPlanDataService, TrainingPlanDataService>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IHttpContextAccessor,  HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.SlidingExpiration = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
        name: "default",
        pattern: "{area=Visitor}/{controller=Home}/{action=Index}/{id?}");

app.Run();
