using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new List<string> { "1", "2" });
        }

        public async Task<IActionResult> Show()
        {
            var trainingPlan = await _trainingPlanService.GetAsync<TrainingPlan>(1);
            return View(trainingPlan);
        }
    }
}
