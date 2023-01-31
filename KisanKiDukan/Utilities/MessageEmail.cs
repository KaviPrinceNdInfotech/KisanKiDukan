using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace KisanKiDukan.Utilities
{
    public class MessageEmail
    {
        public static bool SendEmail(string recipeint, string msg)
        {
            try
            {
                string sender = "care@gyros.farm";
                string password = "nmaitjcngfvrkpnj";
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sender);
                message.To.Add(recipeint);
                message.Subject = "You unlocked something great!";
                message.Body = msg;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(sender, password);
                client.EnableSsl = true;
                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}