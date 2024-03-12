using Microsoft.AspNetCore.Mvc;
using YourTrainerApp2.Data;
using YourTrainerApp2.Models;

namespace YourTrainerApp2.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExerciseController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index1()
        {
            List<Exercise> exerciseList = _db.Exercises.ToList();

            return View(exerciseList);
        }
        public IActionResult Index()
        {
            //var Model = new Models.Exercise
            //{
            //    Response = "Podpiąć api z ćwiczeniami"
            //};
            return View(); 
        }

        public IActionResult Create1()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create1(Exercise obj)
        {
            _db.Exercises.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index1");
        }

        public IActionResult Update1(int? id)
        { 
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Exercise? exerFromDb = _db.Exercises.FirstOrDefault(u => u.Id == id);

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

            _db.Exercises.Update(exerFromDb);
            _db.SaveChanges();
            return RedirectToAction("Index1");
        }

        public IActionResult Delete1(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Exercise? exerFromDb = _db.Exercises.FirstOrDefault(u => u.Id == id);

            if (exerFromDb == null)
            {
                return NotFound();
            }

            return View(exerFromDb);
        }

        [HttpPost, ActionName("Delete1")]
        public IActionResult Delete1POST(int? id)
        {
            Exercise? exerFromDb = _db.Exercises.FirstOrDefault(u => u.Id == id);

            if (exerFromDb is null)
            {
                return NotFound();
            }

            _db.Exercises.Remove(exerFromDb);
            _db.SaveChanges();

            return RedirectToAction("Index1");

        }

        public ActionResult GetDynamicContent(string exerType)
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
                List<Exercise> exercises = _db.Exercises.Where(u => u.PrimaryMuscles == p).ToList();
                foreach (Exercise exercise in exercises)
                {
                    exerList.Add(exercise);
                    exerNames.Add(exercise.Name + "&" + exercise.ImgPath1 + "&" + exercise.Id);
                }
            }
            
            content = string.Join(",", exerNames);

            return Content(content);
        }

        public IActionResult Exercise(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Exercise exerFromDb = _db.Exercises.FirstOrDefault(u => u.Id == id);

            if (exerFromDb == null)
            {
                return NotFound();
            }
            Console.WriteLine(exerFromDb.Instructions);

            return View(exerFromDb);
        }

    }
}
