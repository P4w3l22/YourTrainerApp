using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp2.Models;
using YourTrainerApp2.Services.IServices;

namespace YourTrainerApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ExerciseAdminController : Controller
	{
		private readonly IExerciseService _exerciseService;

		public ExerciseAdminController(IExerciseService exerciseService)
		{
			_exerciseService = exerciseService;
		}

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

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost, ActionName("Create")]
		public async Task<IActionResult> CreatePOST(Exercise exerciseCreated)
		{
			await _exerciseService.CreateAsync<Exercise>(exerciseCreated);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Update(int id)
		{
			if (id == 0)
			{
				return NotFound();
			}

			var apiResponse = await _exerciseService.GetAsync<APIResponse>(id);

			var exercise = JsonConvert.DeserializeObject<Exercise>(Convert.ToString(apiResponse.Result));

			if (exercise is null) return NotFound();

			return View(exercise);
		}

		[HttpPost, ActionName("Update")]
		public IActionResult UpdatePOST(Exercise? exerciseUpdated)
		{
			//Exercise? exerFromDb = _db.Exercises.FirstOrDefault(u => u.Id == id);

			if (exerciseUpdated is null)
			{
				return NotFound();
			}

			_exerciseService.UpdateAsync<Exercise>(exerciseUpdated);
			return RedirectToAction("Index");
		}

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

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeletePOST(int id)
		{
			await _exerciseService.DeleteAsync<Exercise>(id);

			return RedirectToAction("Index");

		}
	}
}
