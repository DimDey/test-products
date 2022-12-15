using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Models;

namespace Products.Controllers
{
    public class AccountController : Controller
    {
        private readonly string adminName;
        private readonly string adminPassword;
        public AccountController(IConfiguration configuration)
        {
            adminName = configuration.GetSection("AdminData").GetValue<string>("User");
            adminPassword = configuration.GetSection("AdminData").GetValue<string>("Password");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                if (model.UserName == adminName && model.Password == adminPassword)
                {
                    await Authenticate(model.UserName);

                    return RedirectToAction("Index", "Product");
                }
                ModelState.AddModelError("", "Некорректные логин или пароль");
            }

            return View(model);
        }


        public async Task Authenticate(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ProductsCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
