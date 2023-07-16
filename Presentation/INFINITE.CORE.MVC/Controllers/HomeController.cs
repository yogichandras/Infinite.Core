using INFINITE.CORE.MVC.Authorization;
using INFINITE.CORE.MVC.Base;
using INFINITE.CORE.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace INFINITE.CORE.MVC.Controllers
{
    [AuthCore]
    public class HomeController : CoreController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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