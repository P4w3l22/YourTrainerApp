using ExerciseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models.DTO;
using YourTrainerApp.Services.IServices;
using YourTrainerApp2.Models;
using YourTrainerApp2.Models.DTO;
using YourTrainerApp2.Services.IServices;

namespace YourTrainerApp.Areas.Visistor.Controllers
{
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

            //var trainingPlans = JsonConvert.DeserializeObject<List<TrainingPlan>>(Convert.ToString(apiResponse.Result))
            //                               .Where(tp => tp.Creator == "admin")
            //                               .ToList();
            
            var trainingPlans = DeserializeApiResult<List<TrainingPlan>>(apiResponse.Result)
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
			var apiResponse = await _trainingPlanService.GetAsync<APIResponse>(id);

			//var trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(Convert.ToString(trainingPlanApiResponse.Result));
			var trainingPlan = DeserializeApiResult<TrainingPlan>(apiResponse.Result);
            trainingPlan.CreateTrainingDaysDict();


            return View(trainingPlan);
        }

        private T DeserializeApiResult<T>(object apiResponseResult) =>
            JsonConvert.DeserializeObject<T>(Convert.ToString(apiResponseResult));
    }
}
