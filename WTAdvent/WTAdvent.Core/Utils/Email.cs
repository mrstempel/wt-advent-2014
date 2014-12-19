using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace WTAdvent.Core.Utils
{
	public static class Email
	{
		public static void Send(MailMessage msg)
		{
			if (Convert.ToBoolean(WebConfigurationManager.AppSettings["Email.Enabled"]))
			{
				if (msg.From == null)
					msg.From = new MailAddress(ConfigurationManager.AppSettings["Email.From"], ConfigurationManager.AppSettings["Email.From.Name"]);
				msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["Email.ReplyTo"], ConfigurationManager.AppSettings["Email.From.Name"]));
				var smtpClient = new SmtpClient();
				smtpClient.Send(msg);
			}
		}

		public static void SendRecommendation(string from, string to, string subject, string content, string ciao = null)
		{
			var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/Templates/Email.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);
			templateContent = templateContent.Replace("{Content}", content);

			var fromAddress = (from == ConfigurationManager.AppSettings["Email.From"])
				? new MailAddress(ConfigurationManager.AppSettings["Email.From"],
					ConfigurationManager.AppSettings["Email.From.Name"])
				: new MailAddress(from);

			var msg = new MailMessage()
			{
				From = fromAddress,
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			msg.To.Add(to);
			Send(msg);
		}
	}
}
