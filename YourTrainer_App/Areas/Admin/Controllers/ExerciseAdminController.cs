using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourTrainer_App.Areas.Admin.Services;
using YourTrainer_Utility;
using YourTrainer_App.Models;
using YourTrainer_App.Models.VM;

namespace YourTrainer_App.Areas.Admin.Controllers;

[Area("Admin")]
public class ExerciseAdminController : Controller
{
	private readonly IExerciseAdminService _exerciseAdminService;

	public ExerciseAdminController(IExerciseAdminService exerciseAdminService)
	{
		_exerciseAdminService = exerciseAdminService;
	}

	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Index()
	{
		List<Exercise> exerciseList = await _exerciseAdminService.GetExercisesList();

		return View(exerciseList);
	}

	[Authorize(Roles = "admin")]
	public IActionResult Create()
	{
		ExerciseCreateVM exerciseCreate = new();
		return View(exerciseCreate);
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExerciseCreateVM exerciseCreated)
	{
		if (ModelState.IsValid)
		{
			(string, string) createResponse = await _exerciseAdminService.CreateExerciseAndGetResponse(exerciseCreated, HttpContext.Session.GetString(StaticDetails.SessionToken));
			string responseType = createResponse.Item1;
			string responseMessage = createResponse.Item2;
			if (responseType == "Error")
			{
				ModelState.AddModelError(string.Empty, responseMessage);
			}
			else
			{
				TempData["success"] = responseMessage;
				return RedirectToAction("Index");
			}
		}

		ExerciseCreateVM exerciseVM = new();
		exerciseVM.Exercise = exerciseCreated.Exercise;
		
		return View(exerciseVM);
	}

	[Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id)
	{
		ExerciseCreateVM exerciseCreateLists = new();
		Exercise exercise = await _exerciseAdminService.GetExercise(id);
		exerciseCreateLists.Exercise = exercise;

		return View(exerciseCreateLists);
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Update(ExerciseCreateVM exerciseUpdated)
	{
		if (ModelState.IsValid)
		{
			(string, string) updateResponse = await _exerciseAdminService.UpdateExerciseAndGetResponse(exerciseUpdated, HttpContext.Session.GetString(StaticDetails.SessionToken));
			string responseType = updateResponse.Item1;
			string responseMessage = updateResponse.Item2;
			if (responseType == "Error")
			{
				ModelState.AddModelError(string.Empty, responseMessage);
			}
			else
			{
				TempData["success"] = responseMessage;
				return RedirectToAction("Index");
			}
		}

		return View(exerciseUpdated);
	}


	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete(int id)
	{
		Exercise exercise = await _exerciseAdminService.GetExercise(id);
		return View(exercise);
	}


    [Authorize(Roles = "admin")]
    [HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeletePOST(int id)
	{
		if (TempData["success"] is not null)
		{
			TempData["success"] = "";
		}
		(string, string) updateResponse = await _exerciseAdminService.DeleteExerciseAndGetResponse(id, HttpContext.Session.GetString(StaticDetails.SessionToken));
		string responseType = updateResponse.Item1;
		string responseMessage = updateResponse.Item2;
		if (responseType == "Error")
		{
			ModelState.AddModelError(string.Empty, responseMessage);
		}
		else
		{
			TempData["success"] = responseMessage;
			return RedirectToAction("Index");
		}

		return View(id);
	}
}
