using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainerApp.Models;

namespace YourTrainerApp.Areas.Trainer.Controllers;

[Area("Trainer")]
public class DataSettingsController : Controller
{
	private readonly ITrainerDataService _trainerDataService;

    private int _trainerId
    {
        get => int.Parse(HttpContext.Session.GetString("UserId"));
    }


	public DataSettingsController(ITrainerDataService trainerDataService)
    {
        _trainerDataService = trainerDataService;
    }

    [HttpGet]
    public async Task<IActionResult> ShowData() =>
        await TrainerDataIsPresent() ? View(await GetTrainerDataFromDb()) : View(GetTrainerDataDefault());

    [HttpPost]
    public async Task<IActionResult> ShowData(TrainerDataModel trainerData)
    {
		if (await TrainerDataIsPresent())
        {
            await _trainerDataService.UpdateAsync<APIResponse>(trainerData);
        }
        else
        {
			APIResponse apiResponse = await _trainerDataService.CreateAsync<APIResponse>(trainerData);
		}

		TempData["success"] = "Zapisano zmiany";
        return RedirectToAction("ShowData");
    }

    public async Task<IActionResult> ClearData()
    {
        APIResponse apiResponse = await _trainerDataService.DeleteAsync<APIResponse>(_trainerId);

        return RedirectToAction("ShowData");
    }

    private async Task<bool> TrainerDataIsPresent()
    {
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(_trainerId);
        return apiResponse.Result is not null;
	}

    private async Task<TrainerDataModel> GetTrainerDataFromDb()
    {
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(_trainerId);
		return JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));
	}

    private TrainerDataModel GetTrainerDataDefault() =>
        new TrainerDataModel()
		{
			TrainerId = _trainerId,
			Email = HttpContext.Session.GetString("Username")
		};
}
