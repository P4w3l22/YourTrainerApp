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
            List<Exercise> exerciseList = _db.Exercises.Take(5).ToList();

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
        public IActionResult Update1POST(int? id)
        {
            Exercise? exerFromDb = _db.Exercises.FirstOrDefault(u => u.Id == id);

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

    }
}
