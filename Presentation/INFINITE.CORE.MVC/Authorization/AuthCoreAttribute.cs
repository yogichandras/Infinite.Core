using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace INFINITE.CORE.MVC.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthCoreAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] permissions;
        public AuthCoreAttribute(params string[] permissions)
        {
            this.permissions = permissions;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHelper = context.HttpContext.RequestServices.GetService<AuthHelper>();
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            
            var account = context.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == authHelper.Configuration["ApplicationConfig:Issuer"]);
            if (string.IsNullOrEmpty(account.Value))
            {
               context.Result = new RedirectToRouteResult(new RouteValueDictionary
               {
                    { "controller", authHelper.Configuration["ApplicationConfig:LoginController"] },
                    { "action", authHelper.Configuration["ApplicationConfig:LoginMethod"] }
               });
            }

            if (permissions != null && permissions.Length > 0)
            {
                var session = authHelper.Session;
                var logoutRoute = new RouteValueDictionary
                {
                   { "controller", authHelper.Configuration["ApplicationConfig:LoginController"] },
                   { "action", authHelper.Configuration["ApplicationConfig:LogoutMethod"] }
                };

                if (session == null)
                {
                    context.Result = new RedirectToRouteResult(logoutRoute);
                }

                if (session.Permissions == null || session.Permissions.Count < 1)
                {
                    context.Result = new RedirectToRouteResult(logoutRoute);
                }

                var isAuthenticated = session.Permissions.Any(x => permissions.ToList().Any(z => z == x));
                if (!isAuthenticated)
                {
                    context.Result = new RedirectToRouteResult(logoutRoute);
                }
            }
        }
    }
}
