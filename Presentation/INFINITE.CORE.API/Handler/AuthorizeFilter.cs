using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using INFINITE.CORE.Shared.Attributes;
using System;
using System.Linq;
using System.Security.Claims;

namespace INFINITE.CORE.API.Handler
{
    public class AuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (SkipAuthorization(context))
            {
                return;
            }
            var response = new ObjectResponse<object>();
            response.UnAuthorized("Authorization has been denied for this request.");
            try
            {
                var identity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null && identity.Claims != null && identity.Claims.Count() > 0)
                {
                    if (identity.Claims.Count(c => c.Type == ClaimTypes.Role) == 0)
                    { 
                        context.HttpContext.Response.StatusCode = 401;
                        context.Result = new JsonResult(response);
                    }
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(response);
                }
                return;
            }
            catch(Exception ex)
            {
                context.HttpContext.Response.StatusCode = 401;
                response.UnAuthorized(ex.Message);
                context.Result = new JsonResult(response);
                return;
            }
        }

        private bool SkipAuthorization(AuthorizationFilterContext context)
        {
            var attributes = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>();

            if (attributes != null && attributes.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
