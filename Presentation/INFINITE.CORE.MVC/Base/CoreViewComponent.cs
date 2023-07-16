using INFINITE.CORE.MVC.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INFINITE.CORE.MVC.Base
{
    public abstract class CoreViewComponent : ViewComponent
    {
        public AuthHelper Auth { get; set; }
    }
}
