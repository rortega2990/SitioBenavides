using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace BenFarms.MVC.Helper
{
    public class EmailHelper
    {
        public static void SendEmail(string email, string subject, string messageBody)
        {
            var smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential
                {
                    UserName = ConfigurationManager.AppSettings["mailuser"],
                    Password = ConfigurationManager.AppSettings["mailpassword"]
                },
                Host = ConfigurationManager.AppSettings["mailserver"],
                Port = int.Parse(ConfigurationManager.AppSettings["mailport"])
            };

            var message = new MailMessage
            {
                To = {new MailAddress(email)},
                From = new MailAddress(ConfigurationManager.AppSettings["mailsystem"]),
                Subject = subject,
                Body = messageBody,
                IsBodyHtml = true
            };

            smtpClient.Send(message);

        }

        public static string RegisterMailBody(string userName)
        {
            return "Gracias por registrarse en Farmacias Benavides";
        }

        public static void SendErrorMail(Exception exc)
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["errormail"]))
            {
                return;
            }

            var smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential
                {
                    UserName = ConfigurationManager.AppSettings["mailuser"],
                    Password = ConfigurationManager.AppSettings["mailpassword"]
                },
                Host = ConfigurationManager.AppSettings["mailserver"],
                Port = int.Parse(ConfigurationManager.AppSettings["mailport"])
            };

            var message = new MailMessage
            {
                To = {new MailAddress(ConfigurationManager.AppSettings["errormail"])},
                From = new MailAddress(ConfigurationManager.AppSettings["mailsystem"]),
                Subject = ConfigurationManager.AppSettings["mailsubject"],
                Body = exc.Message,
                IsBodyHtml = true
            };

            smtpClient.Send(message);
        }
    }
}