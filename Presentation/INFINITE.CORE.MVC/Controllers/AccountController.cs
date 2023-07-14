using INFINITE.CORE.MVC.Base;
using INFINITE.CORE.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace INFINITE.CORE.MVC.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Login()
        {
            var token = Request.Cookies.FirstOrDefault(x => x.Key == Configuration["ApplicationConfig:Issuer"]);
            var validatedToken = Auth.ValidateToken(token.Value);
            if (validatedToken != null)
            {
                return Redirect("/");
            }

            return View(new ErrorViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var validatedToken = Auth.ValidateToken(model.Token);
            if (validatedToken != null)
            {
                var cookieOptions = new CookieOptions
                {
                    Path = "/",
                    Expires = validatedToken.ValidTo
                };
                Response.Cookies.Append(Configuration["ApplicationConfig:Issuer"], model.Token, cookieOptions);
                return Redirect("/");
            }
            else
            {
                return View(new ErrorViewModel { Message = "Username or Password Invalid" });
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete(Configuration["ApplicationConfig:Issuer"]);
            return RedirectToAction(Configuration["ApplicationConfig:LoginUrl"]);
        }
    }
}
