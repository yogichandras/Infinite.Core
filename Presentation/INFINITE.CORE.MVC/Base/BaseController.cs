using INFINITE.CORE.MVC.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INFINITE.CORE.MVC.Base
{
    public class BaseController : Controller
    {
        public IConfiguration Configuration { get; set; }
        public AuthHelper Auth { get; set; }
    }
}
