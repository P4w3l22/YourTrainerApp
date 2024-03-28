using DbDataAccess.Data;
using DbDataAccess.DbAccess;
using ExerciseAPI;
using ExerciseAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDbContext<ApplicationDbContext>(options => {
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
//});

builder.Services.AddControllers();
//builder.Services.AddDbContext<ExerciseContext>(opt =>
//    opt.UseInMemoryDatabase("ExerciseList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IExerciseData, ExerciseData>();
builder.Services.AddSingleton<ITrainingPlanData, TrainingPlanData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
