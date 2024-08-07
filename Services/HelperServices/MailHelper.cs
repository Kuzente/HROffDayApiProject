using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.HelperServices
{
	public class MailHelper
	{
		private readonly IConfiguration _configuration;

		public MailHelper(IConfiguration configuration)
		{

			_configuration = configuration;
		}

		public async Task<bool> SendEmail(string toMail, string subject, string mailHtml)
		{
			try
			{
				var smtpClient = new SmtpClient();
				var smtpSection = _configuration.GetSection("SmtpOptions");
				smtpClient.Host = smtpSection.GetSection("Host").Value;
				smtpClient.Port = Convert.ToInt32(smtpSection.GetSection("Port").Value);
				smtpClient.EnableSsl = true;
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtpClient.UseDefaultCredentials = false;
				smtpClient.Credentials = new NetworkCredential(smtpSection.GetSection("MailAddress").Value, smtpSection.GetSection("Password").Value);

				MailAddress alan = new MailAddress(toMail);
				MailAddress gönderen = new MailAddress(smtpSection.GetSection("MailAddress").Value, "İyaş Personal Takip Sistemi");
				MailMessage eposta = new MailMessage(gönderen, alan);

				eposta.Subject = subject;
				eposta.Body = mailHtml;
				eposta.IsBodyHtml = true;

				await smtpClient.SendMailAsync(eposta);
				return true;
			}
			catch (Exception)
			{
				return false;
			}

		}
		public string GetMailemplateHtml(string template, string firstname, string token, string userId, string userMail, DateTime tokenDate)

		{
			string webRootPath = _configuration.GetSection("RootPath").Value;
			string domainUrl = _configuration.GetSection("DomainUrl").Value;
			string MailingSubDomainRoot = Path.Combine(webRootPath, "mailtemplates", template, "index.html");
			var streamReader = new StreamReader(MailingSubDomainRoot);
			var htmlContent = streamReader.ReadToEnd();
			string Html = (htmlContent).Replace("src=\"assets/", "src=\"" + domainUrl + "/assets/")
										.Replace("{0}", firstname)
										.Replace("{1}", domainUrl)
										.Replace("{2}", token)
										.Replace("{3}", userId)
										.Replace("{4}", userMail)
										.Replace("{5}", tokenDate.ToString("dd MMMM yyyy HH:mm", new System.Globalization.CultureInfo("tr-TR")));
			return Html;
		}
	}
}
