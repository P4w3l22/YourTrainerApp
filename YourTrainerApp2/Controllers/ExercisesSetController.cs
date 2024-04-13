using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models;
using YourTrainerApp2.Models;
using YourTrainerApp2.Services.IServices;

namespace YourTrainerApp.Controllers
{
	public class ExercisesSetController : Controller
	{
		private readonly IExerciseService _exerciseService;

		public ExercisesSetController(IExerciseService exerciseService)
        {
			_exerciseService = exerciseService;
        }

        public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Exercise(int id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var response = await _exerciseService.GetAsync<APIResponse>(id);

			Exercise exercise = JsonConvert.DeserializeObject<Exercise>(Convert.ToString(response.Result));

			if (exercise == null)
			{
				return NotFound();
			}
			Console.WriteLine(exercise.Instructions);

			return View(exercise);
		}

		public async Task<ActionResult> GetDynamicContent(string exerType)
		{
			// abdominals - mięśnie brzucha
			// abductor - odwodziciel ud
			// adductor - przywodziciel ud
			// biceps - biceps
			// calves - łydki
			// chest - klatka
			// forearms - przedramiona
			// glutes - mięśnie pośladków
			// hamstrings - ścięgno podkolanowe
			// lats - mięśnie najszersze grzbietu
			// lower back - dolna część pleców
			// middle back - środkowa część pleców
			// neck - szyja
			// quadriceps - mięśnie czworogłowe (uda)
			// shoulders - barki
			// traps - czworoboczne (plecy)
			// triceps - triceps

			string content = "";
			List<string> pms = new();

			switch (exerType)
			{
				case "chest":
					pms.Add("chest");
					break;
				case "back":
					pms.Add("lats");
					pms.Add("lower back");
					pms.Add("middle back");
					pms.Add("traps");
					pms.Add("neck");
					break;
				case "shoulders":
					pms.Add("shoulders");
					break;
				case "triceps":
					pms.Add("triceps");
					break;
				case "biceps":
					pms.Add("biceps");
					break;
				case "forearms":
					pms.Add("forearms");
					break;
				case "legs":
					pms.Add("abductor");
					pms.Add("adductor");
					pms.Add("glutes");
					pms.Add("hamstrings");
					pms.Add("quadriceps");
					break;
				case "abdominals":
					pms.Add("abdominals");
					break;
				case "calves":
					pms.Add("calves");
					break;
				default:
					break;
			}

			List<Exercise> exerList = new();
			List<string> exerNames = new();

			foreach (string p in pms)
			{
				var exers = await _exerciseService.GetAllAsync<APIResponse>();

				List<Exercise> exercises = JsonConvert.DeserializeObject<List<Exercise>>(Convert.ToString(exers.Result))
													  .Where(u => u.PrimaryMuscles == p).ToList();
				foreach (Exercise exercise in exercises)
				{
					exerList.Add(exercise);
					exerNames.Add(exercise.Name + "&" + exercise.ImgPath1 + "&" + exercise.Id);
				}
			}

			content = string.Join(",", exerNames);

			return Content(content);
		}
	}
}
