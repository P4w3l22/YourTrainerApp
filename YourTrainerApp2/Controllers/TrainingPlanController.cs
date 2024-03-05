using Microsoft.AspNetCore.Mvc;
using YourTrainerApp2.Models;

namespace YourTrainerApp2.Controllers
{
    public class TrainingPlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new List<string> { "1", "2" });
        }

        [HttpPost]
        public IActionResult Create(List<string> trDays)
        {
            TrainingPlan newPlan = new();
            newPlan.Days = trDays;
            return View(trDays);
        }

        public IActionResult Show()
        {
            return View();
        }
    }
}
