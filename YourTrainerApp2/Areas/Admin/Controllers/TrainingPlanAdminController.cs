using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Services.IServices;
using YourTrainerApp2.Models;

namespace YourTrainerApp.Areas.Admin.Controllers;

[Area("Admin")]
public class TrainingPlanAdminController : Controller
{
    private readonly ITrainingPlanService _trainingPlanService;

    public TrainingPlanAdminController(ITrainingPlanService trainingPlanService)
    {
        _trainingPlanService = trainingPlanService;
    }

    public async Task<IActionResult> Index()
    {
        var apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();
        var plans = JsonConvert.DeserializeObject<List<TrainingPlan>>(Convert.ToString(apiResponse.Result));

        return View(plans);
    }
}
