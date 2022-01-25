using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FarmFresh.Backend.Web.Customer.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home", null, Request.Scheme)

            });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home", null, Request.Scheme)

                },
                    OpenIdConnectDefaults.AuthenticationScheme,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
        }

        public IActionResult AccessDenied() => View();
    }
}
