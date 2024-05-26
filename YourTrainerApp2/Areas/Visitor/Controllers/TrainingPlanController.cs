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

		if (TempData["TrainingPlanData"] is null ||
			TempData["TrainingPlanData"].ToString().Length == 0)
		{
			string trainingPlanInJson = JsonConvert.SerializeObject(trainingPlan);
			var trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);

			TempData["TrainingPlanData"] = trainingPlanInJson;
		}
		else
		{
			string trainingPlanInJson = TempData["TrainingPlanData"] as string;
			TrainingPlan trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);
		}

		return View(trainingPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TrainingPlan trainingPlan)
    {
        if (trainingPlan.TrainingDaysDict["Wtorek"])
        {
            Console.WriteLine("Działa");
        }

        HttpContext.Session.SetString("ExercisesId", "");

        if (TempData["TrainingPlanData"] is null ||
            TempData["TrainingPlanData"].ToString().Length == 0)
        {
            string trainingPlanInJson = JsonConvert.SerializeObject(trainingPlan);
            var trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);

            TempData["TrainingPlanData"] = trainingPlanInJson;
        }
        else
        {
            string trainingPlanInJson = TempData["TrainingPlanData"] as string;
            TrainingPlan trainingPlanObject = JsonConvert.DeserializeObject<TrainingPlan>(trainingPlanInJson);  

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

        string trainingPlanInJson = TempData["TrainingPlanData"] as string;
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

		TempData["TrainingPlanData"] = JsonConvert.SerializeObject(trainingPlanObject);

		return View();
    }

    private T DeserializeApiResult<T>(object apiResponseResult) =>
        JsonConvert.DeserializeObject<T>(Convert.ToString(apiResponseResult));
}
