using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class AuthenticationController : Controller
{
    

    // GET
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(string username , string password)
    {
        if (username == "samicangulcan" && password == "samicangulcan")
        {
            List<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "samicangulcan"),
                new Claim(ClaimTypes.Role, "admin")
            };
            var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Kalıcı bir cookie oluştur
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60) // Cookie'nin 30 dakika sonra geçerliliğini yitir
            };
            await HttpContext.SignOutAsync(authProperties);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }else if (username == "azimyilmaz" && password == "azimyilmaz")
        {
            List<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "azimyilmaz"),
                new Claim(ClaimTypes.Role, "admin")
            };
            var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Kalıcı bir cookie oluştur
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60) // Cookie'nin 30 dakika sonra geçerliliğini yitir
            };
            await HttpContext.SignOutAsync(authProperties);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }else if (username == "celalettincalis" && password == "celalettincalis")
        {
            List<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "celalettincalis"),
                new Claim(ClaimTypes.Role, "admin")
            };
            var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Kalıcı bir cookie oluştur
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60) // Cookie'nin 30 dakika sonra geçerliliğini yitir
            };
            await HttpContext.SignOutAsync(authProperties);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);  
        }
        return Redirect("/");
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/giris-yap");
    }
   
}