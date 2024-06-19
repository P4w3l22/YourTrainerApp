using Newtonsoft.Json;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainerApp.Models;
using YourTrainerApp.Models.VM;

namespace YourTrainer_App.Areas.Admin.Services;

public class ExerciseAdminDataService
{
	private readonly IExerciseService _exerciseService;

	public ExerciseAdminDataService(IExerciseService exerciseService)
	{
		_exerciseService = exerciseService;
	}

	public async Task<List<Exercise>> GetExercisesList()
	{
		List<Exercise> exerciseList = new();

		var apiResponse = await _exerciseService.GetAllAsync<APIResponse>();

		if (apiResponse is not null && apiResponse.IsSuccess)
		{
			exerciseList = JsonConvert.DeserializeObject<List<Exercise>>(Convert.ToString(apiResponse.Result));
		}

		return exerciseList;
	}

	//public async Task CreateExercise(bool isValid, ExerciseCreateVM exerciseCreated)
	//{
	//	var apiResponse = await _exerciseService.CreateAsync<APIResponse>(exerciseCreated.Exercise, HttpContext.Session.GetString(StaticDetails.SessionToken));

	//	if (ModelState.IsValid)
	//	{
	//		if (apiResponse is not null || apiResponse.IsSuccess)
	//		{
	//			TempData["success"] = "Utworzono ćwiczenie!";
	//			return RedirectToAction("Index");
	//		}
	//	}
	//	else
	//	{
	//		if (apiResponse.Errors is not null)
	//		{
	//			ModelState.AddModelError("Errors", apiResponse.Errors.FirstOrDefault());
	//		}
	//	}

	//	ExerciseCreateVM exerciseVM = new();
	//	exerciseVM.Exercise = exerciseCreated.Exercise;
	//}
}
