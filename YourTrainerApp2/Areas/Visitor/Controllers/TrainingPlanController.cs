using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models.DTO;
using YourTrainerApp.Services.IServices;
using YourTrainerApp.Models;
using YourTrainerApp.Models.VM;
using YourTrainerApp.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace YourTrainerApp.Areas.Visistor.Controllers;

[Area("Visitor")]
public class TrainingPlanController : Controller
{
    private readonly ITrainingPlanService _trainingPlanService;
    private readonly ITrainingPlanExerciseService _trainingPlanExerciseService;
    private readonly IExerciseService _exerciseService;
    
    private string? _sessionUsername 
    {
        get => HttpContext.Session.GetString("Username");
        set => HttpContext.Session.SetString("Username", value); 
    }
    private TrainingPlan? _sessionTrainingPlan
    {
        get => JsonConvert.DeserializeObject<TrainingPlan>(HttpContext.Session.GetString("TrainingPlanData") ?? JsonConvert.SerializeObject(new TrainingPlan()));
        set => HttpContext.Session.SetString("TrainingPlanData", JsonConvert.SerializeObject(value));
    }
    private List<TrainingPlanExerciseCreateVM>? _sessionExercises
	{
		get => JsonConvert.DeserializeObject<List<TrainingPlanExerciseCreateVM>>(HttpContext.Session.GetString("Exercises") ?? JsonConvert.SerializeObject(new List<TrainingPlanExerciseCreateVM>()));
		set => HttpContext.Session.SetString("Exercises", JsonConvert.SerializeObject(value));
	}

	public TrainingPlanController(ITrainingPlanService trainingPlanService, ITrainingPlanExerciseService trainingPlanExerciseService, IExerciseService exerciseService)
	{
        _trainingPlanService = trainingPlanService;
		_trainingPlanExerciseService = trainingPlanExerciseService;
		_exerciseService = exerciseService;
	}

    public async Task<IActionResult> Index()
    {
        var apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();

		if (_sessionUsername is null ||
			_sessionUsername.Length == 0)
        {
			_sessionUsername = "admin";
        }

        var trainingPlans = DeserializeResult<List<TrainingPlan>>(apiResponse.Result)
                           .Where(tp => tp.Creator == _sessionUsername)
                           .ToList();

        if (_sessionExercises is not null)
        {
            _sessionExercises = new();
		}

        return View(trainingPlans);
    }

    [HttpGet]
    public IActionResult Create(bool isCreating = true)
    {
        if (!isCreating)
        {
			_sessionExercises = new();
		}

        TrainingPlan trainingPlan = new();
        trainingPlan.Creator = _sessionUsername;

        // Przyporządkowywanie do zmiennej sesji planu treningowego lub odczytywanie go jeśli jest ustawiony
		if (_sessionTrainingPlan is null)
		{
			_sessionTrainingPlan = trainingPlan;
		}
		else
		{
            trainingPlan = _sessionTrainingPlan;
		}

		if (_sessionExercises.IsNullOrEmpty())
        {
			_sessionExercises = new();
		}

		return View(trainingPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ClearSessionStrings]
    public async Task<IActionResult> Create(TrainingPlan tp)
    {
        TrainingPlan trainingPlan = _sessionTrainingPlan;
		trainingPlan.CreateTrainingDaysString();

        if (trainingPlan.Creator is null)
        {
            trainingPlan.Creator = _sessionUsername;
        }

        await _trainingPlanService.CreateAsync<APIResponse>(trainingPlan);

        var apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();
        TrainingPlan? trainingPlanDb = DeserializeResult<List<TrainingPlan>>(apiResponse.Result)
						              .Where(tp => tp.Title == trainingPlan.Title && tp.Creator == trainingPlan.Creator)
						              .FirstOrDefault();
        // Błąd wczytywania danych nowego planu treningowego z bazy
        int id = trainingPlanDb.Id;

        foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan.Exercises)
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
		var trainingPlan = DeserializeResult<TrainingPlan>(apiResponse.Result);
        trainingPlan.CreateTrainingDaysDict();

		List<TrainingPlanExerciseCreateVM> exercises = new();


		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan.Exercises)
        {
			var apiResponse2 = await _exerciseService.GetAsync<APIResponse>(trainingPlanExercise.EId);
            TrainingPlanExerciseCreateVM exercise = DeserializeResult<TrainingPlanExerciseCreateVM>(apiResponse2.Result);
            exercises.Add(exercise);
		}

        _sessionExercises = exercises;


		return View(trainingPlan);
    }

    public async Task<IActionResult> DeleteTrainingPlan(int id)
    {
        await _trainingPlanService.DeleteAsync<APIResponse>(id);

        TempData["success"] = "Usunięto plan treningowy";

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ExerciseSelectionAsync() =>
		View();
    

    public IActionResult IncrementExerciseSeries(int id)
    {
		TrainingPlan trainingPlan = _sessionTrainingPlan;

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

		_sessionTrainingPlan = trainingPlan;

		return RedirectToAction("Create");
	}

    public IActionResult DecrementExerciseSeries(int id)
    {
		TrainingPlan trainingPlan = _sessionTrainingPlan;

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

		_sessionTrainingPlan = trainingPlan;

		return RedirectToAction("Create");
	}

    [HttpGet]
    public IActionResult SaveRepsAndWeightsData(string values, string exerciseId, string seriesPosition)
    {
        int id = int.Parse(exerciseId);
        int seriesId = int.Parse(seriesPosition);
        string[] repsAndWeights = values.Split(';');

		TrainingPlan trainingPlan = _sessionTrainingPlan;

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

				break;
			}
		}

        _sessionTrainingPlan = trainingPlan;

		return Ok();
    }

	public async Task<IActionResult> AddExerciseId(int id)
    {
        List<TrainingPlanExerciseCreateVM> exercises = _sessionExercises;

        if (exercises is null)
        {
            exercises = new();
        }

        TrainingPlan trainingPlan = _sessionTrainingPlan;

        var apiResponse = await _exerciseService.GetAsync<APIResponse>(id);
		if (apiResponse is not null && apiResponse.IsSuccess)
		{
			TrainingPlanExerciseCreateVM exercise = DeserializeResult<TrainingPlanExerciseCreateVM>(apiResponse.Result);
		    exercises.Add(exercise);

			_sessionExercises = exercises;

            if (trainingPlan.Exercises is null)
            {
                trainingPlan.Exercises = new();
            }

            trainingPlan.Exercises.Add(new()
            {
                EId = exercise.Id,
                Series = 1,
                Reps = "4",
                Weights = "80"
            });

            _sessionTrainingPlan = trainingPlan;
        }

		return RedirectToAction("Create");
	}


    public IActionResult SaveTitleData(string title)
    {
        TrainingPlan trainingPlan = _sessionTrainingPlan;
        trainingPlan.Title = title;
        _sessionTrainingPlan = trainingPlan;
        return Ok();
    }

    public IActionResult SaveTrainigDaysData(string day)
    {
		TrainingPlan trainingPlan = _sessionTrainingPlan;
		if (trainingPlan.TrainingDaysDict[day])
        {
			trainingPlan.TrainingDaysDict[day] = false;
		}
        else
        {
            trainingPlan.TrainingDaysDict[day] = true;
        }

		_sessionTrainingPlan = trainingPlan;

		return Ok();
    }

    public IActionResult SaveNotesData(string notes)
    {
        TrainingPlan trainingPlan = _sessionTrainingPlan;
        trainingPlan.Notes = notes;
		_sessionTrainingPlan = trainingPlan;
        return Ok();
    }

    private T DeserializeResult<T>(object apiResponseResult) =>
        JsonConvert.DeserializeObject<T>(Convert.ToString(apiResponseResult));
}
