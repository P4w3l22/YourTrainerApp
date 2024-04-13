using Azure;
using ExerciseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp2.Models;
using YourTrainerApp2.Services.IServices;
using Exercise = YourTrainerApp2.Models.Exercise;

namespace YourTrainerApp2.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task<IActionResult> Index()
        {
            List<Exercise> exerciseList = new();
                
            var response = await _exerciseService.GetAllAsync<APIResponse>();
            if (response is not null && response.IsSuccess)
            {
                exerciseList = JsonConvert.DeserializeObject<List<Exercise>>(Convert.ToString(response.Result));
            }

            return View(exerciseList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePOST(Exercise obj)
        {
            await _exerciseService.CreateAsync<Exercise>(obj);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == 0) return NotFound();
            
            var response = await _exerciseService.GetAsync<APIResponse>(id);

            var exercise = JsonConvert.DeserializeObject<Exercise>(Convert.ToString(response.Result));

            if (exercise is null) return NotFound();

            return View(exercise);
        }

        [HttpPost, ActionName("Update")]
        public IActionResult UpdatePOST(Exercise? exerFromDb)
        {
            //Exercise? exerFromDb = _db.Exercises.FirstOrDefault(u => u.Id == id);

            if (exerFromDb is null) return NotFound();

            _exerciseService.UpdateAsync<Exercise>(exerFromDb);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
			var response = await _exerciseService.GetAsync<APIResponse>(id);

			var exercise = JsonConvert.DeserializeObject<Exercise>(Convert.ToString(response.Result));

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
