using System.Security.Claims;
using System.Text.Json;
using Core.DTOs;
using Core.DTOs.UserDtos.ReadDtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.UserServices;
using Services.HelperServices;
using UI.Models;

namespace UI.Controllers;

public class AuthenticationController : Controller
{
    private const string LoginRequestsKey = "AdminLoginRequests"; // bu appsettings üzerinden gelecek
    private readonly IConfiguration _configuration;
    private readonly RecaptchaVerifyHelper _recaptchaVerifyHelper;
    private readonly IReadUserService _readUserService;

    public AuthenticationController(RecaptchaVerifyHelper recaptchaVerifyHelper, IReadUserService readUserService, IConfiguration configuration)
    {
        _recaptchaVerifyHelper = recaptchaVerifyHelper;
        _readUserService = readUserService;
        _configuration = configuration;
    }

    // GET
    public IActionResult Login()
    {
        if (HttpContext.User.Identity.IsAuthenticated) return Redirect("/");
        ViewBag.sitekey = _configuration["reCAPTCHA:sitekey"];
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(ReadUserSignInDto dto)
    {
        IResultWithDataDto<ReadUserSignInDto> res = new ResultWithDataDto<ReadUserSignInDto>();
        if (HttpContext.User.Identity.IsAuthenticated) return Ok(res.SetStatus(false).SetErr("Session Is Exist").SetMessage("Kullanıcının oturumu zaten açık!"));
        var adminLoginRequests = GetAdminLoginRequest();
        if (!ModelState.IsValid) return Ok(res.SetStatus(false).SetErr("Not found Parameters").SetMessage("Lütfen tüm alanları girdiğinize emin olun!"));
        if (adminLoginRequests.IsRestricted && adminLoginRequests.LastRequestDate.AddMinutes(5) > DateTime.Now) return Ok(res.SetStatus(false).SetErr("Too Many Attr").SetMessage("Birden Fazla Yanlış Deneme Yaptınız Lütfen 5 dakika sonra tekrar deneyiniz!!!"));
        if (await _recaptchaVerifyHelper.IsGoogleReCaptchaVerify(dto.Security) is false) return Ok(res.SetStatus(false).SetErr("Recaptcha Error").SetMessage("Güvenlik doğrulaması sağlanamadı! Lütfen sayfayı yenileyerek tekrar deneyin!"));
         if (adminLoginRequests.IsRestricted) ResetLoginRequests();
        res = await _readUserService.SignInService(dto);
         if (!res.IsSuccess)
         {
             AddLoginRequest();
             return Ok(res);
         }
        ResetLoginRequests();
        List<Claim> userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,res.Data.ID.ToString()),
            new Claim(ClaimTypes.Name,res.Data.Username),
            new Claim(ClaimTypes.Email,res.Data.Email),
            new Claim(ClaimTypes.Role, res.Data.Role.ToString())
        };
        var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true, // remember me 
            ExpiresUtc = DateTime.UtcNow.AddMinutes(120) // Cookie'nin 30 dakika sonra geçerliliğini yitir
        };
        await HttpContext.SignOutAsync();
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
        return Ok(res);
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/giris-yap");
    }
    #region Private Methods
    /// <summary>
    /// Hatalı giriş işlemlerini kontrol eden method
    /// </summary>
    /// <returns></returns>
    private LoginRequests GetAdminLoginRequest()
    {
        var adminJson = HttpContext.Session.GetString(LoginRequestsKey);
        if (string.IsNullOrEmpty(adminJson))
        {
            return ResetLoginRequests(); // veya başka bir varsayılan değeri döndürün
        }
        return JsonSerializer.Deserialize<LoginRequests>(adminJson);
    }
    /// <summary>
    /// Hatalı girişleri sıfırlayan method
    /// </summary>
    /// <returns></returns>
    private LoginRequests ResetLoginRequests()
    {
        var adminLoginRequests = new LoginRequests
        {
            RequestNumber = 0,
            IsRestricted = false
        };
        HttpContext.Session.SetString(LoginRequestsKey, JsonSerializer.Serialize(adminLoginRequests));
        return adminLoginRequests;
    }
    /// <summary>
    /// Hatalı girişleri ekleme işlemlerini yapan method
    /// </summary>
    private void AddLoginRequest()
    {
        var adminLoginRequests = GetAdminLoginRequest();
        adminLoginRequests.RequestNumber++;
        adminLoginRequests.LastRequestDate = DateTime.Now;
        if (adminLoginRequests.RequestNumber == 5)
            adminLoginRequests.IsRestricted = true;

        HttpContext.Session.SetString(LoginRequestsKey, JsonSerializer.Serialize(adminLoginRequests));
    }
    #endregion
}