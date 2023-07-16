using INFINITE.CORE.Data.Provider;
using INFINITE.CORE.MVC.Authorization;
using INFINITE.CORE.MVC.Base;
using Microsoft.AspNetCore.Mvc;

namespace INFINITE.CORE.MVC.Controllers
{
    [AuthCore(PermissionNames.Pages_Home)]
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
    }
}