using Microsoft.AspNetCore.Mvc;

namespace INFINITE.CORE.MVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
