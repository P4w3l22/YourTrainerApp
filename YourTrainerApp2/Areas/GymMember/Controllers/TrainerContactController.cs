using Microsoft.AspNetCore.Mvc;

namespace YourTrainerApp.Areas.GymMember.Controllers;

[Area("GymMember")]
public class TrainerContactController : Controller
{
	public IActionResult Index()
	{
		List<string> names = new() { "Paweł", "Kacper", "Michał", "Piotr", "Patryk", "Szymon" };

		return View(names);
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
