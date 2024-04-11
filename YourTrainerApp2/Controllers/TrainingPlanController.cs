using DbDataAccess.Models;
using ExerciseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Services.IServices;
using YourTrainerApp2.Models;
using YourTrainerApp2.Services.IServices;

namespace YourTrainerApp2.Controllers
{
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

            var trainingPlans = JsonConvert.DeserializeObject<List<TrainingPlan>>(Convert.ToString(apiResponse.Result))
                                           .Where(tp => tp.Creator == "admin")
                                           .ToList();

            return View(trainingPlans);
        }

        public IActionResult Create()
        {
            return View(new List<string> { "1", "2" });
        }

        public async Task<IActionResult> Show(int id)
        {
            var trainingPlanApiResponse = await _trainingPlanService.GetAsync<APIResponse>(id);

            var trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(Convert.ToString(trainingPlanApiResponse.Result));

            Dictionary<string, bool> trainingDays = new();
            string[] splitedTrainingDaysDb = trainingPlan.TrainingDays.Split(';');
            foreach (string day in splitedTrainingDaysDb)
            {
                List<string> dayKeyValue = day.Split(':').ToList();
                trainingDays.Add(dayKeyValue[0], dayKeyValue[1] == "0" ? false : true);
            }

            trainingPlan.TrainingDaysDict = trainingDays;
            trainingPlan.Exercises = new();

            var trainingPlanExerciseApiResponse = await _trainingPlanExerciseService.GetAllAsync<APIResponse>(id);

			var trainingPlanExercises = JsonConvert.DeserializeObject<List<TrainingPlanExercise>>(Convert.ToString(trainingPlanExerciseApiResponse.Result));

            foreach (var trainingPlanExercise in trainingPlanExercises)
            {
				var exerciseApiResponse = await _exerciseService.GetAsync<APIResponse>(id);

				var exercise = JsonConvert.DeserializeObject<Models.Exercise>(Convert.ToString(exerciseApiResponse.Result));

                var t = "test";
                trainingPlan.Exercises.Add(exercise);
            }

			return View(trainingPlan);
        }
    }
}
