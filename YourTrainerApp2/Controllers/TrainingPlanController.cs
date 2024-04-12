using DbDataAccess.Models;
using ExerciseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models.DTO;
using YourTrainerApp.Services.IServices;
using YourTrainerApp2.Models;
using YourTrainerApp2.Models.DTO;
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

            var trainingPlans = JsonConvert.DeserializeObject<List<TrainingPlanModel>>(Convert.ToString(apiResponse.Result))
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
			//         var trainingPlanApiResponse = await _trainingPlanService.GetAsync<APIResponse>(id);

			//         var trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(Convert.ToString(trainingPlanApiResponse.Result));


			//         trainingPlan.Exercises = new();
			//         List<TrainingPlanExerciseDTO> trainingPlanExercisesData = new();

			//         var trainingPlanExerciseApiResponse = await _trainingPlanExerciseService.GetAllAsync<APIResponse>(id);
			//var trainingPlanExercises = JsonConvert.DeserializeObject<List<TrainingPlanExercise>>(Convert.ToString(trainingPlanExerciseApiResponse.Result));

			//         foreach (var trainingPlanExercise in trainingPlanExercises)
			//         {
			//	var exerciseApiResponse = await _exerciseService.GetAsync<APIResponse>(id);

			//	var exercise = JsonConvert.DeserializeObject<Models.Exercise>(Convert.ToString(exerciseApiResponse.Result));


			//             var planExercise = new TrainingPlanExerciseDTO(trainingPlanExercise.Weights)
			//             {
			//                 Id = trainingPlanExercise.Id,
			//                 Name = exercise.Name,
			//                 ImgPath = exercise.ImgPath1,
			//                 Series = trainingPlanExercise.Series
			//             };

			//             trainingPlanExercisesData.Add(planExercise);
			//         }

			//         ViewBag.TrainingPlan = trainingPlan;
			//         ViewBag.TrainingPlanExercises = trainingPlanExercisesData;

			var trainingPlanApiResponse = await _trainingPlanService.GetAsync<APIResponse>(id);

			var trainingPlan = JsonConvert.DeserializeObject<TrainingPlanModel>(Convert.ToString(trainingPlanApiResponse.Result));

			trainingPlan.TrainingDaysDict = new();
			string[] splitedTrainingDaysDb = trainingPlan.TrainingDays.Split(';');
			foreach (string day in splitedTrainingDaysDb)
			{
				List<string> dayKeyValue = day.Split(':').ToList();
				trainingPlan.TrainingDaysDict.Add(dayKeyValue[0], dayKeyValue[1] == "0" ? false : true);
			}

			return View(trainingPlan);
        }
    }
}
