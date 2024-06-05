using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;

namespace YourTrainerApp.Areas.GymMember.Controllers;

[Area("GymMember")]
public class TrainerContactController : Controller
{
	private readonly ITrainerDataService _trainerDataService;

    public TrainerContactController(ITrainerDataService trainerDataService)
    {
		_trainerDataService = trainerDataService;
    }

    public async Task<IActionResult> Index()
	{
		APIResponse apiResponse = await _trainerDataService.GetAllAsync<APIResponse>();

		List<TrainerDataModel> trainersData = JsonConvert.DeserializeObject<List<TrainerDataModel>>(Convert.ToString(apiResponse.Result));


        List<string> names = new() { "Paweł", "Kacper", "Michał", "Piotr", "Patryk", "Szymon" };

		return View(trainersData);
	}

	public IActionResult TrainerMessages()
	{
		return View();
	}

	public IActionResult AddTrainer() 
	{ 
		return View(); 
	}

	public IActionResult TrainerSelection()
	{
		return View();
	}

	public IActionResult AssignedTrainingPlan()
	{
		return View();
	}



}
