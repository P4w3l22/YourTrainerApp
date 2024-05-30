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
using Microsoft.Extensions.Logging.Abstractions;
using YourTrainerApp.Attributes;
using Microsoft.AspNetCore.Mvc.Filters;

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
        // Dodać w konstruktorze przyporządkowanie zmiennej sesji
    }

    public async Task<IActionResult> Index()
    {
        var apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();
        string creator = HttpContext.Session.GetString("Username");

        string test = HttpContext.Session.GetString("TrainingPlanData");
        string test2 = HttpContext.Session.GetString("Exercises");

		if (creator is null ||
			creator.Length == 0)
        {
            creator = "admin";
        }

        var trainingPlans = DeserializeApiResult<List<TrainingPlan>>(apiResponse.Result)
                           .Where(tp => tp.Creator == creator)
                           .ToList();

        if (HttpContext.Session.GetString("Exercises") is not null)
        {
            HttpContext.Session.SetString("Exercises", "");
		}

        return View(trainingPlans);
    }

    [HttpGet]
    public IActionResult Create(bool isCreating = true)
    {
        if (!isCreating)
        {
            HttpContext.Session.SetString("Exercises", "");
		}

        TrainingPlan trainingPlan = new();
        trainingPlan.Creator = HttpContext.Session.GetString("Username");

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
		if (HttpContext.Session.GetString("Exercises") is null ||
			HttpContext.Session.GetString("Exercises").Length == 0)
        {
            HttpContext.Session.SetString("Exercises", JsonConvert.SerializeObject(new List<TrainingPlanExerciseCreateVM>()));
		}

		return View(trainingPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ClearSessionStrings]
    public async Task<IActionResult> Create(TrainingPlan trainingPlan)
    {
        TrainingPlan trainingPlan1 = GetTrainingPlanSessionData();

        trainingPlan1.CreateTrainingDaysString();

        // Do refaktoryzacji

        await _trainingPlanService.CreateAsync<APIResponse>(trainingPlan1);

        var apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();
        TrainingPlan? trainingPlanDb = DeserializeApiResult<List<TrainingPlan>>(apiResponse.Result)
						                  .Where(tp => tp.Title == trainingPlan1.Title && tp.Creator == trainingPlan1.Creator)
						                  .FirstOrDefault();

        int id = trainingPlanDb.Id;

        foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan1.Exercises)
        {
            await _trainingPlanExerciseService.InsertAsync<APIResponse>(new TrainingPlanExerciseCreateDTO()
            {
                TPId = id,
                EId = trainingPlanExercise.EId,
                Series = trainingPlanExercise.Series,
                Reps = trainingPlanExercise.Reps,
                Weights = trainingPlanExercise.Weights
            });
        }

		TempData["success"] = "Dodano plan!";

		return RedirectToAction("Index");
	}

	//[ClearSessionStrings]
	public async Task<IActionResult> Show(int id)
    {
		var apiResponse = await _trainingPlanService.GetAsync<APIResponse>(id);

		//var trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(Convert.ToString(trainingPlanApiResponse.Result));
		var trainingPlan = DeserializeApiResult<TrainingPlan>(apiResponse.Result);
        trainingPlan.CreateTrainingDaysDict();

		List<TrainingPlanExerciseCreateVM> exercises = new();


		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan.Exercises)
        {
			var apiResponse2 = await _exerciseService.GetAsync<APIResponse>(trainingPlanExercise.EId);
            TrainingPlanExerciseCreateVM exercise = JsonConvert.DeserializeObject<TrainingPlanExerciseCreateVM>(Convert.ToString(apiResponse2.Result));
            exercises.Add(exercise);
		}

        string exercisesInJson = JsonConvert.SerializeObject(exercises);
        HttpContext.Session.SetString("Exercises", exercisesInJson);


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
                trainingPlanExercise.Reps += ";4";
                trainingPlanExercise.Weights += ";80";

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

                    string[] reps = trainingPlanExercise.Reps.Split(";");
                    string[] weights = trainingPlanExercise.Weights.Split(";");

                    trainingPlanExercise.Reps = "";
                    trainingPlanExercise.Weights = "";

                    for (int i = 0; i < trainingPlanExercise.Series; i++)
                    {
                        trainingPlanExercise.Reps += reps[i] + ';';
                        trainingPlanExercise.Weights += weights[i] + ';';
					}

                    if (trainingPlanExercise.Weights.Length > 0)
                    {
						trainingPlanExercise.Reps = trainingPlanExercise.Reps.Substring(0, trainingPlanExercise.Reps.Length-1);
						trainingPlanExercise.Weights = trainingPlanExercise.Weights.Substring(0, trainingPlanExercise.Weights.Length-1);
					}

				}
				
				break;
			}
		}

		SaveTrainingPlanSessionData(trainingPlan);

		return RedirectToAction("Create");
	}

    [HttpGet]
    public IActionResult SaveRepsAndWeightsData(string values, string exerciseId, string seriesPosition)
    {
        int id = int.Parse(exerciseId);
        int seriesId = int.Parse(seriesPosition);
        string[] repsAndWeights = values.Split(';');

		TrainingPlan trainingPlan = GetTrainingPlanSessionData();

		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan.Exercises)
		{
			if (trainingPlanExercise.EId == id)
			{
				string[] reps = trainingPlanExercise.Reps.Split(";");
				string[] weights = trainingPlanExercise.Weights.Split(";");

                trainingPlanExercise.Reps = "";
                trainingPlanExercise.Weights = "";

				reps[seriesId] = repsAndWeights[0].ToString();
				weights[seriesId] = repsAndWeights[1].ToString(); 

				for (int i = 0; i < trainingPlanExercise.Series; i++)
				{
					trainingPlanExercise.Reps += reps[i] + ';';
					trainingPlanExercise.Weights += weights[i] + ';';
				}

				if (trainingPlanExercise.Weights.Length > 0)
				{
					trainingPlanExercise.Reps = trainingPlanExercise.Reps.Substring(0, trainingPlanExercise.Reps.Length - 1);
					trainingPlanExercise.Weights = trainingPlanExercise.Weights.Substring(0, trainingPlanExercise.Weights.Length - 1);
				}

				//trainingPlanExercise.Reps += ";" + values[0];
    //            trainingPlanExercise.Weights += ";" + values[1];

				break;
			}
		}

        SaveTrainingPlanSessionData(trainingPlan);

		return Ok("Test");
    }

	public async Task<IActionResult> AddExerciseId(int id)
    {
        string exercisesInJson = HttpContext.Session.GetString("Exercises");
        List<TrainingPlanExerciseCreateVM> exercises = JsonConvert.DeserializeObject<List<TrainingPlanExerciseCreateVM>>(exercisesInJson);

        if (exercises is null)
        {
            exercises = new();
        }

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
                Series = 3,
                Reps = "4;4;4",
                Weights = "80;80;80"
            });

            SaveTrainingPlanSessionData(trainingPlan);
        }

        //TrainingPlan trainingPlan = GetTrainingPlanSessionData();
		return RedirectToAction("Create");
	}


    public IActionResult SaveTitleData(string title)
    {
        TrainingPlan trainingPlan = GetTrainingPlanSessionData();
        trainingPlan.Title = title;
        SaveTrainingPlanSessionData(trainingPlan);
        return Ok();
    }

    public IActionResult SaveTrainigDaysData(string day)
    {
        TrainingPlan trainingPlan = GetTrainingPlanSessionData();
        if (trainingPlan.TrainingDaysDict[day])
        {
			trainingPlan.TrainingDaysDict[day] = false;
		}
        else
        {
            trainingPlan.TrainingDaysDict[day] = true;
        }
		
        SaveTrainingPlanSessionData(trainingPlan);

        return Ok();
    }

    public IActionResult SaveNotesData(string notes)
    {
        TrainingPlan trainingPlan = GetTrainingPlanSessionData();
        trainingPlan.Notes = notes;
        SaveTrainingPlanSessionData(trainingPlan);
        return Ok();
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
