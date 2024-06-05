using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;

namespace YourTrainerApp.Areas.Trainer.Controllers;

[Area("Trainer")]
public class DataSettingsController : Controller
{
	private readonly ITrainerDataService _trainerDataService;
    
    private int _trainerId;


	public DataSettingsController(ITrainerDataService trainerDataService)
    {
        _trainerDataService = trainerDataService;
    }

    public async Task<IActionResult> ShowData()
    {
        _trainerId = int.Parse(HttpContext.Session.GetString("UserId"));

        APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(_trainerId);
        TrainerDataModel trainerData = JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));

        if (trainerData is null)
        {
            return View(new TrainerDataModel());
        }

        return View(trainerData);
	}

    [HttpPost]
    public async Task<IActionResult> ShowData(TrainerDataModel trainerData)
    {
        if (trainerData is null)
        {
            TempData["error"] = "Wypełnij poprawnie dane";
            return View(new TrainerDataModel());
        }

        await _trainerDataService.CreateAsync<APIResponse>(trainerData);

        TempData["success"] = "Zapisano zmiany";
        return RedirectToAction("Index", "Home", new { Area = "Visitor" });
    }
}
