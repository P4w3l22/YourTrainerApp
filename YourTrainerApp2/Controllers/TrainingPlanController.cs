using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        public IActionResult Show()
        {
            return View();
        }
    }
}
