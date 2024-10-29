using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TestMailServices
{
	public interface IEmailService
	{
	Task<bool> SendEmailAsync(string toMail, string subject, string mailHtml);
	Task<string> GetMailemplateHtml(string template, string firstname, string token, string userId, string userMail, DateTime tokenDate);
	}
}
