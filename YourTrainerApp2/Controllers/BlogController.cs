using Microsoft.AspNetCore.Mvc;
using YourTrainerApp2.Models;

namespace YourTrainerApp2.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            var Post1 = new BlogViewModel();
            Post1.Category = "Kategoria wpisu";
            Post1.Title = "Tytuł posta";
            Post1.Users = new List<string> { "użytkownik1", "użytkownik2", "użytkownik3", "użytkownik4" };
            Post1.Comments = new List<string> { "wpis1", "wpis2", "wpis3", "wpis4" };

            return View(Post1);
        }
    }
}
