using System.Diagnostics;
using CharityJamLights.Models;
using CharityJamLights.Services;
using Microsoft.AspNetCore.Mvc;

namespace CharityJamLights.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LightEngine _lightEngine;

        public HomeController(ILogger<HomeController> logger, LightEngine lightEngine)
        {
            _logger = logger;
            _lightEngine = lightEngine ?? throw new ArgumentNullException(nameof(lightEngine));
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Test()
        {
            await _lightEngine.Send();
            return RedirectToAction(nameof(Index));
        }
    }
}
