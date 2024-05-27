//using DbDataAccess.Models;
//using ExerciseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models.DTO;
using YourTrainerApp.Services.IServices;
using YourTrainerApp.Models;
using YourTrainerApp.Models.DTO;
using YourTrainerApp.Services.IServices;

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
        
		if (HttpContext.Session.GetString("TrainingPlanData") is null ||
            HttpContext.Session.GetString("TrainingPlanData").ToString().Length == 0)
		{
			string trainingPlanInJson = JsonConvert.SerializeObject(trainingPlan);
			var trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);

            HttpContext.Session.SetString("TrainingPlanData", trainingPlanInJson);
		}
		else
		{
			string trainingPlanInJson = HttpContext.Session.GetString("TrainingPlanData");
			TrainingPlan trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);
		}

		return View(trainingPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TrainingPlan trainingPlan)
    {
        //if (trainingPlan.TrainingDaysDict["Wtorek"])
        //{
        //    Console.WriteLine("Działa");
        //}

        string trainingPlanInJson = string.Empty;
        TrainingPlan trainingPlanObject = new();

        HttpContext.Session.SetString("ExercisesId", "");

        if (HttpContext.Session.GetString("TrainingPlanData") is null ||
            HttpContext.Session.GetString("TrainingPlanData").ToString().Length == 0)
        {
            trainingPlanInJson = JsonConvert.SerializeObject(trainingPlan);
			trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);

            HttpContext.Session.SetString("TrainingPlanData", trainingPlanInJson);
        }
        else
        {
            trainingPlanInJson = HttpContext.Session.GetString("TrainingPlanData");
            trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);
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

    public async Task<IActionResult> ExerciseSelectionAsync()
    {
        //string exercisesId = HttpContext.Session.GetString("ExercisesId");

        //exercisesId += "1040;1041";

        //HttpContext.Session.SetString("ExercisesId", exercisesId);

        string trainingPlanInJson = HttpContext.Session.GetString("TrainingPlanData");
        TrainingPlan trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);

        trainingPlanObject.Exercises =
		[
			new TrainingPlanExercise()
			{
				Id = 20,
				TpId = 1,
				EId = 1040,
				Series = 3,
				Reps = "10;10;12",
				Weights = "50;50;60"
			},
		];

		HttpContext.Session.SetString("TrainingPlanData", JsonConvert.SerializeObject(trainingPlanObject));

		return View();
    }

    public IActionResult AddExerciseId(int id)
    {
        var trainingPlan = HttpContext.Session.GetString("TrainingPlanData");
		string trainingPlanInJson = JsonConvert.SerializeObject(trainingPlan);
		TrainingPlan trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);
        //trainingPlanObject.TrainingDays

		HttpContext.Session.SetString("TrainingPlanData", trainingPlanInJson);
        return RedirectToAction("Create");
	}

    private T DeserializeApiResult<T>(object apiResponseResult) =>
        JsonConvert.DeserializeObject<T>(Convert.ToString(apiResponseResult));
}
