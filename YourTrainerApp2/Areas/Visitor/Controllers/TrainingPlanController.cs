//using DbDataAccess.Models;
//using ExerciseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models.DTO;
using YourTrainerApp.Services.IServices;
using YourTrainerApp.Models;
using YourTrainerApp.Models.DTO;
using YourTrainerApp.Services.IServices;
using YourTrainerApp.Models.VM;

namespace YourTrainerApp.Areas.Visistor.Controllers;

[Area("Visitor")]
public class TrainingPlanController : Controller
{
    private readonly ITrainingPlanService _trainingPlanService;
    private readonly ITrainingPlanExerciseService _trainingPlanExerciseService;
    private readonly IExerciseService _exerciseService;

    public TrainingPlanController(ITrainingPlanService trainingPlanService, ITrainingPlanExerciseService trainingPlanExerciseService, IExerciseService exerciseService)
	{
        _trainingPlanService = trainingPlanService;
		_trainingPlanExerciseService = trainingPlanExerciseService;
		_exerciseService = exerciseService;
    }

    public async Task<IActionResult> Index()
    {
        var apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();
        
        var trainingPlans = DeserializeApiResult<List<TrainingPlan>>(apiResponse.Result)
                           .Where(tp => tp.Creator == "admin")
                           .ToList();

        return View(trainingPlans);
    }

    [HttpGet]
    public IActionResult Create()
    {
        TrainingPlan trainingPlan = new();

        // Przyporządkowywanie do zmiennej sesji planu treningowego lub odczytywanie go jeśli jest ustawiony
		if (HttpContext.Session.GetString("TrainingPlanData") is null ||
			HttpContext.Session.GetString("TrainingPlanData").ToString().Length == 0)
		{
			string trainingPlanInJson = JsonConvert.SerializeObject(trainingPlan);

			HttpContext.Session.SetString("TrainingPlanData", trainingPlanInJson);
		}
		else
		{
			string trainingPlanInJson = HttpContext.Session.GetString("TrainingPlanData");
			trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);
		}

        // Utworzenie zmiennej sesji zawierającej id ćwiczeń
		if (HttpContext.Session.GetString("Exercises") is null)
        {
            HttpContext.Session.SetString("Exercises", JsonConvert.SerializeObject(new List<TrainingPlanExerciseCreateVM>()));
		}

		return View(trainingPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TrainingPlan trainingPlan)
    {
		// TODO: dodać kod obsługujący dodawanie ćwiczeń na podstawie id ze zmiennej sesji do trainingPlan

		string trainingPlanInJson = string.Empty;

		if (HttpContext.Session.GetString("TrainingPlanData") is null ||
			HttpContext.Session.GetString("TrainingPlanData").ToString().Length == 0)
		{
			trainingPlanInJson = JsonConvert.SerializeObject(trainingPlan);

			HttpContext.Session.SetString("TrainingPlanData", trainingPlanInJson);
		}
		else
		{
			trainingPlanInJson = HttpContext.Session.GetString("TrainingPlanData");
			trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);
		}

		return View(trainingPlan);
    }

    public async Task<IActionResult> Show(int id)
    {
		var apiResponse = await _trainingPlanService.GetAsync<APIResponse>(id);

		//var trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(Convert.ToString(trainingPlanApiResponse.Result));
		var trainingPlan = DeserializeApiResult<TrainingPlan>(apiResponse.Result);
        trainingPlan.CreateTrainingDaysDict();


        return View(trainingPlan);
    }

    public async Task<IActionResult> ExerciseSelectionAsync() =>
		View();
    

    public IActionResult IncrementExerciseSeries(int id)
    {
		TrainingPlan trainingPlan = GetTrainingPlanSessionData();

        foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan.Exercises)
        {
            if (trainingPlanExercise.EId == id)
            {
                trainingPlanExercise.Series++;
                break;
            }
        }

        SaveTrainingPlanSessionData(trainingPlan);

		return RedirectToAction("Create");
	}

    public IActionResult DecrementExerciseSeries(int id)
    {
		TrainingPlan trainingPlan = GetTrainingPlanSessionData();

		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan.Exercises)
		{
			if (trainingPlanExercise.EId == id)
			{
                if (trainingPlanExercise.Series > 0)
                {
                    trainingPlanExercise.Series--;
                }
				
				break;
			}
		}

		SaveTrainingPlanSessionData(trainingPlan);

		return RedirectToAction("Create");
	}

	public async Task<IActionResult> AddExerciseId(int id)
    {
        string exercisesInJson = HttpContext.Session.GetString("Exercises");
        List<TrainingPlanExerciseCreateVM> exercises = JsonConvert.DeserializeObject<List<TrainingPlanExerciseCreateVM>>(exercisesInJson);

        TrainingPlan trainingPlan = GetTrainingPlanSessionData();

        var apiResponse = await _exerciseService.GetAsync<APIResponse>(id);
		if (apiResponse is not null && apiResponse.IsSuccess)
		{
			TrainingPlanExerciseCreateVM exercise = JsonConvert.DeserializeObject<TrainingPlanExerciseCreateVM>(Convert.ToString(apiResponse.Result));
		    exercises.Add(exercise);

			HttpContext.Session.SetString("Exercises", JsonConvert.SerializeObject(exercises));

            if (trainingPlan.Exercises is null)
            {
                trainingPlan.Exercises = new();
            }

            trainingPlan.Exercises.Add(new()
            {
                EId = exercise.Id,
                Series = 3
            });

            SaveTrainingPlanSessionData(trainingPlan);
        }

        //TrainingPlan trainingPlan = GetTrainingPlanSessionData();
		return RedirectToAction("Create");
	}

    private void SaveTrainingPlanSessionData(TrainingPlan trainingPlan)
    {
		string trainingPlanInJson = JsonConvert.SerializeObject(trainingPlan);
        HttpContext.Session.SetString("TrainingPlanData", trainingPlanInJson);	
    }

    private TrainingPlan GetTrainingPlanSessionData() =>
		JsonConvert.DeserializeObject<TrainingPlan>(HttpContext.Session.GetString("TrainingPlanData"));

    private T DeserializeApiResult<T>(object apiResponseResult) =>
        JsonConvert.DeserializeObject<T>(Convert.ToString(apiResponseResult));
}
