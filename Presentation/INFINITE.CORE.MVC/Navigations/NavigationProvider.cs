using INFINITE.CORE.Data.Provider;

namespace INFINITE.CORE.MVC.Navigations
{
    public class NavigationProvider
    {
        public NavigationContext ListMenu()
        {
            return new NavigationContext()
                .AddItem(
                    new NavigationDefinition(
                        title: Pages.Home,
                        url: "Home",
                        icon: "fas fa-home",
                        requiredPermissions: new List<string> { PermissionNames.Pages_Home }
                    )
                )
                .AddItem(
                    new NavigationDefinition(
                        title: Pages.Users,
                        url: "User",
                        icon: "fas fa-users",
                        requiredPermissions: new List<string> { PermissionNames.Pages_Users }
                    )
                ).AddItem(
                    new NavigationDefinition(
                        title: Pages.Roles,
                        url: "Role",
                        icon: "fas fa-theater-masks",
                        requiredPermissions: new List<string> { PermissionNames.Pages_Roles }
                    )
                ).AddItem(
                    new NavigationDefinition(
                        title: Pages.Config,
                        url: "#",
                        icon: "fas fa-cogs",
                        requiredPermissions: new List<string> { PermissionNames.Pages_Config, PermissionNames.Pages_Email_Template }
                    ).AddItem(
                        new NavigationDefinition(
                            title: Pages.AppConfig,
                            url: "Config",
                            icon: "",
                            requiredPermissions: new List<string> { PermissionNames.Pages_Config }
                        )
                    ).AddItem(
                        new NavigationDefinition(
                            title: Pages.EmailTemplate,
                            url: "EmailTemplate",
                            icon: "",
                            requiredPermissions: new List<string> { PermissionNames.Pages_Email_Template }
                        )
                    )
                );
        }

    }
}
