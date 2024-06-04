using DbDataAccess.Data;
using DbDataAccess.DbAccess;
using ExerciseAPI;
using ExerciseAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDbContext<ApplicationDbContext>(options => {
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
//});

var key = builder.Configuration.GetValue<string>("ApiSettings:SecretToken");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();
//builder.Services.AddDbContext<ExerciseContext>(opt =>
//    opt.UseInMemoryDatabase("ExerciseList"));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
            new List<string>()
        }
    });
});

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IExerciseData, ExerciseData>();
builder.Services.AddScoped<ITrainingPlanData, TrainingPlanData>();
builder.Services.AddScoped<ILocalUserData, LocalUserData>();
builder.Services.AddScoped<ITrainerData, TrainerData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
