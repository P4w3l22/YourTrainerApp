using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using YourTrainerApp.Models;
using Exercise = YourTrainerApp.Models.Exercise;

namespace YourTrainerApp.Areas.Visistor.Controllers
{
    [Area("Visitor")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _client;

        public HomeController(IHttpClientFactory client, ILogger<HomeController> logger)
        {
			_client = client;
			_logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var client = _client.CreateClient();
            var url = "https://localhost:7051/api/ExerciseAPI/1";

            var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();
            //var result = JsonConvert.DeserializeObject<APIResponse>(content);
            var output = JsonConvert.DeserializeObject<Exercise>(content);

			return View(output);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
