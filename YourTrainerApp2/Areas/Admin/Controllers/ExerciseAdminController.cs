using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models.VM;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using YourTrainerApp.Attributes;

namespace YourTrainerApp.Areas.Admin.Controllers;

[Area("Admin")]
public class ExerciseAdminController : Controller
{
	private readonly IExerciseService _exerciseService;

	public ExerciseAdminController(IExerciseService exerciseService)
	{
		_exerciseService = exerciseService;
	}

	[AdminSessionCheck]
	public async Task<IActionResult> Index()
	{
		List<Exercise> exerciseList = new();

		var apiResponse = await _exerciseService.GetAllAsync<APIResponse>();

		if (apiResponse is not null && apiResponse.IsSuccess)
		{
			exerciseList = JsonConvert.DeserializeObject<List<Exercise>>(Convert.ToString(apiResponse.Result));
		}

		return View(exerciseList);
	}


    [AdminSessionCheck]
    public IActionResult Create() =>
		View(new ExerciseCreateVM());


	[AdminSessionCheck]
	[HttpPost, ActionName("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePOST(ExerciseCreateVM exerciseCreated)
	{
		var apiResponse = await _exerciseService.CreateAsync<APIResponse>(exerciseCreated.Exercise);

		if (ModelState.IsValid)
		{
			if (apiResponse is not null || apiResponse.IsSuccess)
			{
				TempData["success"] = "Utworzono ćwiczenie!";
				return RedirectToAction("Index");
			}	
		}
		else
        {
            if (apiResponse.Errors is not null)
			{
				ModelState.AddModelError("Errors", apiResponse.Errors.FirstOrDefault());
			}
        }

		ExerciseCreateVM exerciseVM = new();
		exerciseVM.Exercise = exerciseCreated.Exercise;
		
		return View(exerciseVM);
	}


    [AdminSessionCheck]
    public async Task<IActionResult> Update(int id)
	{

		ExerciseCreateVM exerciseCreateLists = new();

		if (id == 0)
		{
			return NotFound();
		}

		var apiResponse = await _exerciseService.GetAsync<APIResponse>(id);

		var exercise = JsonConvert.DeserializeObject<Exercise>(Convert.ToString(apiResponse.Result));

		if (exercise is null)
		{
			return NotFound();
		}

		exerciseCreateLists.Exercise = exercise;

		return View(exerciseCreateLists);
	}


	[AdminSessionCheck]
    [HttpPost, ActionName("Update")]
	public async Task<IActionResult> UpdatePOST(ExerciseCreateVM exerciseUpdated)
	{
		if (exerciseUpdated is null)
		{
			return NotFound();
		}

        await _exerciseService.UpdateAsync<APIResponse>(exerciseUpdated.Exercise);
        TempData["success"] = "Zaktualizowano ćwiczenie!";
        return RedirectToAction("Index");
	}


    [AdminSessionCheck]
    public async Task<IActionResult> Delete(int id)
	{
		if (id == 0)
		{
			return NotFound();
		}

		var apiResponse = await _exerciseService.GetAsync<APIResponse>(id);

		var exercise = JsonConvert.DeserializeObject<Exercise>(Convert.ToString(apiResponse.Result));

		if (exercise == null)
		{
			return NotFound();
		}

		return View(exercise);
	}


    [AdminSessionCheck]
    [HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeletePOST(int id)
	{
		await _exerciseService.DeleteAsync<APIResponse>(id);
        TempData["success"] = "Usunięto ćwiczenie!";
        return RedirectToAction("Index");
	}
}
