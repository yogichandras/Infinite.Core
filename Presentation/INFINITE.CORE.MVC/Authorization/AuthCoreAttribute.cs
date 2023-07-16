using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Principal;

namespace INFINITE.CORE.MVC.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthCoreAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHelper = context.HttpContext.RequestServices.GetService<AuthHelper>();
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            
            // authorization
            var account = context.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == authHelper.Configuration["ApplicationConfig:Issuer"]);
            if (string.IsNullOrEmpty(account.Value))
            {
                // not logged in or role not authorized
               context.Result = new RedirectToRouteResult(new RouteValueDictionary
               {
                    { "controller", authHelper.Configuration["ApplicationConfig:LoginController"] },
                    { "action", authHelper.Configuration["ApplicationConfig:LoginMethod"] }
               });
            }
        }
    }
}
