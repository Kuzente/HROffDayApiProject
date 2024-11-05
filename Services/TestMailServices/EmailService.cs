using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MimeKit;
using MimeKit.Text;

namespace Services.TestMailServices;

public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;
	private readonly IHostEnvironment _hostEnvironment;

	public EmailService(IConfiguration configuration, IHostEnvironment hostEnvironment)
	{
		_configuration = configuration;
		_hostEnvironment = hostEnvironment;
	}

	public async Task<string> GetMailemplateHtml(string template, string firstname, string token, string userId, string userMail, DateTime tokenDate)
	{
		try
		{
			string webRootPath = _configuration.GetSection("RootPath").Value;
			string domainUrl = _configuration.GetSection("DomainUrl").Value;
			string MailingSubDomainRoot = Path.Combine(webRootPath, "mailtemplates", template, "index.html");
			using var streamReader = new StreamReader(MailingSubDomainRoot);
			var htmlContent = await streamReader.ReadToEndAsync();
			string Html = (htmlContent).Replace("src=\"assets/", "src=\"" + domainUrl + "/assets/")
										.Replace("{0}", firstname)
										.Replace("{1}", domainUrl)
										.Replace("{2}", token)
										.Replace("{3}", userId)
										.Replace("{4}", userMail)
										.Replace("{5}", tokenDate.ToString("dd MMMM yyyy HH:mm", new System.Globalization.CultureInfo("tr-TR")));
			return Html;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public async Task<bool> SendEmailAsync(string toMail, string subject, string mailHtml)
	{
		try
		{
			var smtpSection = _configuration.GetSection("SmtpOptions");
			string host = smtpSection.GetSection("Host").Value;
			int port = Convert.ToInt32(smtpSection.GetSection("Port").Value);
			string username = smtpSection.GetSection("MailAddress").Value;
			string password = smtpSection.GetSection("Password").Value;
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(username));
			email.To.Add(MailboxAddress.Parse(toMail));
			email.Subject = subject;
			email.Body = new TextPart(TextFormat.Html)
			{
				Text = mailHtml,
			};
			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(username, password);
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
