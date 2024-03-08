using Microsoft.AspNetCore.Mvc;
using YourTrainerApp2.Data;
using YourTrainerApp2.Models;

namespace YourTrainerApp2.Areas.Admin.Controllers
{
	public class ExercisesController : Controller
	{
		private readonly ApplicationDbContext _db;

        public ExercisesController(ApplicationDbContext db)
        {
			_db = db;
        }

        public IActionResult Index()
		{
			//List<Exercise> exerciseList = _db.Exercises.ToList();

			return View();
		}
		public IActionResult Delete()
		{
			return View();
		}
		public IActionResult Upsert()
		{
			return View();
		}
	}
}
