using INFINITE.CORE.MVC.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INFINITE.CORE.MVC.Base
{
    public class CoreController : Controller
    {
        public required IConfiguration Configuration { get; set; }
        public required AuthHelper Auth { get; set; }
    }
}
