using INFINITE.CORE.MVC.Authorization;
using INFINITE.CORE.MVC.Base;
using INFINITE.CORE.MVC.Navigations;
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
                Session = Auth.Session,
                Navigations = new NavigationProvider().ListMenu()
            };
            return View(model);
        }
    }
}
