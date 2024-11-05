using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Core;
using Core.DTOs;
using Core.DTOs.AuthenticationDtos.ReadDtos;
using Core.DTOs.UserDtos.ReadDtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract.UserServices;
using Services.HelperServices;
using UI.Helpers;
using UI.Models;

namespace UI.Controllers;
public class AuthenticationController : BaseController
{
    private const string LoginRequestsKey = "AdminLoginRequests"; // bu appsettings üzerinden gelecek
    private readonly IConfiguration _configuration;
    private readonly RecaptchaVerifyHelper _recaptchaVerifyHelper;
    private readonly IReadUserService _readUserService;
    private readonly IWriteUserService _writeUserService;
	private readonly JwtHelper _jwtHelper;

	public AuthenticationController(RecaptchaVerifyHelper recaptchaVerifyHelper, IReadUserService readUserService, IConfiguration configuration, IWriteUserService writeUserService, JwtHelper jwtHelper)
	{
		_recaptchaVerifyHelper = recaptchaVerifyHelper;
		_readUserService = readUserService;
		_configuration = configuration;
		_writeUserService = writeUserService;
		_jwtHelper = jwtHelper;
	}

	#region ViewActions
	public IActionResult Login()
	{
		if (HttpContext.User.Identity.IsAuthenticated)
			return Redirect("/");
		ViewBag.sitekey = _configuration["reCAPTCHA:sitekey"];
		return View();
	}
	// GET
	public IActionResult ForgotPassword()
	{
		if (HttpContext.User.Identity.IsAuthenticated)
			return Redirect("/");
		ViewBag.sitekey = _configuration["reCAPTCHA:sitekey"];
		return View();
	}
	public async Task<IActionResult> NewPassword(ConfirmForgotPassDto dto)
	{
		if (HttpContext.User.Identity.IsAuthenticated)
			return Redirect("/");//todo
		var result = await _writeUserService.ForgotPasswordConfirmEmailService(dto);
		ViewBag.sitekey = _configuration["reCAPTCHA:sitekey"];
		return View(result);
	}
	#endregion
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ForgotPasswordPost(ForgotPasswordPostDto dto)
	{
		if (HttpContext.User.Identity.IsAuthenticated)
			return Redirect("/");
		IResultDto res = new ResultDto();
		if (!ModelState.IsValid)
		{
			return Ok(res.SetStatus(false).SetErr("ModelState is not Valid").SetMessage("Lütfen tüm alanları girdiğinize emin olun!"));
		}
		if (await _recaptchaVerifyHelper.IsGoogleReCaptchaVerify(dto.Security) is false)
			return Ok(res.SetStatus(false).SetErr("Recaptcha Error").SetMessage("Güvenlik doğrulaması sağlanamadı! Lütfen sayfayı yenileyerek tekrar deneyin!"));
		res = await _writeUserService.ForgotPasswordService(dto, GetClientIpAddress());
		return Ok(res);
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
	{
		if (HttpContext.User.Identity.IsAuthenticated) return Redirect("/");
		IResultDto res = new ResultDto();
        if (!ModelState.IsValid)
        {
			var error = ModelState.Values.SelectMany(v => v.Errors)
									  .Select(e => e.ErrorMessage)
									  .First();
			return Ok(res.SetStatus(false).SetErr("ModelState is not Valid").SetMessage(error));
		} 
		if (await _recaptchaVerifyHelper.IsGoogleReCaptchaVerify(dto.Security) is false) return Ok(res.SetStatus(false).SetErr("Recaptcha Error").SetMessage("Güvenlik doğrulaması sağlanamadı! Lütfen sayfayı yenileyerek tekrar deneyin!"));
		res = await _writeUserService.ResetPasswordService(dto,GetClientIpAddress());
		return Ok(res);
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
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
		// JWT oluştur
		List<Claim> userClaims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
			new(ClaimTypes.NameIdentifier,res.Data.ID.ToString()),
			new(ClaimTypes.Name,res.Data.Username),
			new(ClaimTypes.Email,res.Data.Email),
			new(ClaimTypes.Role, res.Data.Role.ToString())
		};
		var token = _jwtHelper.GenerateJwtToken(userClaims);
		// JWT'yi HTTP-Only ve Secure cookie olarak sakla
		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Secure = true, // HTTPS gerektirir
			Expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["JwtOptions:ExpirePerHour"])),
			SameSite = SameSiteMode.Strict,
		};
		Response.Cookies.Append(_configuration["JwtOptions:JwtCookieName"]!, token, cookieOptions);
		return Ok(res);
    }
    public async Task<IActionResult> Logout()
    {
		Response.Cookies.Delete(_configuration["JwtOptions:JwtCookieName"]!);
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