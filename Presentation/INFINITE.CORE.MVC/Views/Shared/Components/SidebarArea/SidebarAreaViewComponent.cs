using INFINITE.CORE.MVC.Authorization;
using INFINITE.CORE.MVC.Base;
using Microsoft.AspNetCore.Mvc;

namespace INFINITE.CORE.MVC.Views.Shared.Components.SidebarArea
{
    public class SidebarAreaViewComponent : CoreViewComponent
    {
        public SidebarAreaViewComponent(AuthHelper Auth)
        {
            this.Auth = Auth;
        }
        public IViewComponentResult Invoke()
        {
            var model = new SidebarAreaViewModel
            {
                Session = Auth.Session
            };
            return View(model);
        }
    }
}
