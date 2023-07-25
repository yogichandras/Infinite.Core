using INFINITE.CORE.Data.Provider;
using INFINITE.CORE.MVC.Authorization;
using INFINITE.CORE.MVC.Base;
using Microsoft.AspNetCore.Mvc;

namespace INFINITE.CORE.MVC.Controllers
{
    [AuthCore(PermissionNames.Pages_Email_Template)]
    public class EmailTemplateController : CoreController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
