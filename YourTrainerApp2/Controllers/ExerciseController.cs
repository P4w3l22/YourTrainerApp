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
        //private readonly ApplicationDbContext _db;
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task<IActionResult> Index1()
        {
            List<Exercise> exerciseList = new();
                
            var response = await _exerciseService.GetAllAsync<APIResponse>();
            if (response is not null && response.IsSuccess)
            {
                exerciseList = JsonConvert.DeserializeObject<List<Exercise>>(Convert.ToString(response.Result));
            }

            return View(exerciseList);
        }
        public IActionResult Index()
        {
            return View(); 
        }

        public IActionResult Create1()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create1(Exercise obj)
        {
            await _exerciseService.CreateAsync<Exercise>(obj);
            return RedirectToAction("Index1");
        }

        public async Task<IActionResult> Update1(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Exercise? exerFromDb = await _exerciseService.GetAsync<Exercise>(id);

            if (exerFromDb is null)
            {
                return NotFound();
            }

            return View(exerFromDb);
        }

        [HttpPost, ActionName("Update1")]
        public IActionResult Update1POST(Exercise? exerFromDb)
        {
            //Exercise? exerFromDb = _db.Exercises.FirstOrDefault(u => u.Id == id);

            if (exerFromDb is null)
            {
                return NotFound();
            }

            _exerciseService.UpdateAsync<Exercise>(exerFromDb);
            return RedirectToAction("Index1");
        }

        public async Task<IActionResult> Delete1(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Exercise? exerFromDb = await _exerciseService.GetAsync<Exercise>(id);

            if (exerFromDb == null)
            {
                return NotFound();
            }

            return View(exerFromDb);
        }

        [HttpPost, ActionName("Delete1")]
        public async Task<IActionResult> Delete1POST(int id)
        {
            await _exerciseService.DeleteAsync<Exercise>(id);

            return RedirectToAction("Index1");

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

        public async Task<IActionResult> Exercise(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Exercise exerFromDb = await _exerciseService.GetAsync<Exercise>(id);

            if (exerFromDb == null)
            {
                return NotFound();
            }
            Console.WriteLine(exerFromDb.Instructions);

            return View(exerFromDb);
        }

    }
}
