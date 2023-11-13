using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using News.Areas.Admin.ViewModels;
using News.Common;
using News.Context;
using System.Security.Claims;
using System.Text;
using System.Web.Helpers;

namespace News.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly NewsDbContext _context;
        private readonly VerifyHashPassword _verifyPassword;

        public AccountController(NewsDbContext context, VerifyHashPassword verifyPassword)
        {
            _context = context;
            _verifyPassword = verifyPassword;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModels viewModels)
        {
            var loginUser = _context.Users.Where(x => x.UserName == viewModels.UserName).FirstOrDefault();

            if (loginUser != null)
            {

                var comparePassword = _verifyPassword.VerifyHashedPassword(loginUser.Password, viewModels.Password);
                if (comparePassword)
                {

                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role,"Admin"),
                        new Claim(ClaimTypes.Name,viewModels.UserName),
                    };

                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddDays(1)
                    });
                }
                else
                {
                    ModelState.AddModelError("", "نام کاربری یا رمز وارد شده نامعتبر است.");
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
