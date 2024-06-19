using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models;
using YourTrainerApp.Attributes;
using YourTrainer_App.Services.APIServices.IServices;

namespace YourTrainerApp.Areas.Admin.Controllers;

[Area("Admin")]
public class TrainingPlanAdminController : Controller
{
    private readonly ITrainingPlanService _trainingPlanService;

    public TrainingPlanAdminController(ITrainingPlanService trainingPlanService)
    {
        _trainingPlanService = trainingPlanService;
    }


    [AdminSessionCheck]
    public async Task<IActionResult> Index()
    {
        var apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();
        var plans = JsonConvert.DeserializeObject<List<TrainingPlan>>(Convert.ToString(apiResponse.Result));

        return View(plans);
    }
}
