﻿using Microsoft.AspNetCore.Mvc;

namespace YourTrainerApp2.Controllers
{
    public class ExerciseController : Controller
    {
        public IActionResult Index()
        {
            var Model = new Models.ExerciseViewModel
            {
                Response = "Podpiąć api z ćwiczeniami"
            };
            return View(Model);
        }

    }
}
