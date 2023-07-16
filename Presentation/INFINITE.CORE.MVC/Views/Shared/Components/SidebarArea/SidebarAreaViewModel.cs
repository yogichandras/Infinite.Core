using INFINITE.CORE.MVC.Authorization;
using INFINITE.CORE.MVC.Navigations;

namespace INFINITE.CORE.MVC.Views.Shared.Components.SidebarArea
{
    public class SidebarAreaViewModel
    {
        public CoreSession Session { get; set; }
        public NavigationContext Navigations { get; set; }
    }
}
