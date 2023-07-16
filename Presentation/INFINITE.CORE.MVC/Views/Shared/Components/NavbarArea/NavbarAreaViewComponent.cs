using INFINITE.CORE.MVC.Authorization;
using INFINITE.CORE.MVC.Base;
using Microsoft.AspNetCore.Mvc;

namespace INFINITE.CORE.MVC.Views.Shared.Components.NavbarArea
{
    public class NavbarAreaViewComponent : CoreViewComponent
    {
        public NavbarAreaViewComponent(AuthHelper Auth)
        {
            this.Auth = Auth;
        }
        public IViewComponentResult Invoke()
        {
            var model = new NavbarAreaViewModel
            {
                Session = Auth.Session
            };
            return View(model);
        }
    }
}
