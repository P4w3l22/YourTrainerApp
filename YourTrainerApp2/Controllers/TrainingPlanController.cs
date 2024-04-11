using DbDataAccess.Models;
using ExerciseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Services.IServices;
using YourTrainerApp2.Models;

namespace YourTrainerApp2.Controllers
{
    public class TrainingPlanController : Controller
    {
        private readonly ITrainingPlanService _trainingPlanService;
        private readonly ITrainingPlanExerciseService _trainingPlanExerciseService;

        public TrainingPlanController(ITrainingPlanService trainingPlanService, ITrainingPlanExerciseService trainingPlanExerciseService)
		{
            _trainingPlanService = trainingPlanService;
			_trainingPlanExerciseService = trainingPlanExerciseService;
		}

        public async Task<IActionResult> Index()
        {
            var apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();

            var trainingPlans = JsonConvert.DeserializeObject<List<TrainingPlan>>(Convert.ToString(apiResponse.Result))
                                           .Where(tp => tp.Creator == "admin")
                                           .ToList();

            return View(trainingPlans);
        }

        public IActionResult Create()
        {
            return View(new List<string> { "1", "2" });
        }

        public async Task<IActionResult> Show()
        {
            var apiResponse = await _trainingPlanService.GetAsync<APIResponse>(3);

            var trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(Convert.ToString(apiResponse.Result));

            Dictionary<string, bool> trainingDays = new();
            string[] splitedTrainingDaysDb = trainingPlan.TrainingDays.Split(';');
            foreach (string day in splitedTrainingDaysDb)
            {
                List<string> dayKeyValue = day.Split(':').ToList();
                trainingDays.Add(dayKeyValue[0], dayKeyValue[1] == "0" ? false : true);
            }

            trainingPlan.TrainingDaysDict = trainingDays;

            return View(trainingPlan);
        }
    }
}
