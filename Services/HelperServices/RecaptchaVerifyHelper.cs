using System.Text.Json;
using Core.Helpers;
using Microsoft.Extensions.Configuration;

namespace Services.HelperServices;

public class RecaptchaVerifyHelper
{
    private readonly IConfiguration _configuration;

    public RecaptchaVerifyHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<bool> IsGoogleReCaptchaVerify(string sitekey)
    {
        using var client = new HttpClient();
        var response = await client.GetStringAsync($@"{_configuration["reCAPTCHA:siteUrl"]}?secret={_configuration["reCAPTCHA:secretkey"]}&response={sitekey}");
        var recaptchaResponse = JsonSerializer.Deserialize<RecaptchaResponse>(response);
        if (!recaptchaResponse.success || recaptchaResponse is null) return false;
        return true;
    }
}