using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace KisanKiDukan.Utilities
{
    public class EmailOperations
    {
        public static bool SendEmail(string recipeint, string subject, string msg, bool IsBodyHtml)
        {
            try
            {
                string sender = ConfigurationManager.AppSettings["smtpUser"];
                string password = ConfigurationManager.AppSettings["smtpPass"];
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sender);
                message.To.Add(recipeint);
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = IsBodyHtml;
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


        public static bool SendOrderEmail(string Name, string Email, string MobileNumber, string Address, double TotalAmount, DateTime? OrderDate, IEnumerable<OrderDetailModel> OrderDetails)
        {
            try
            {
                string sender = ConfigurationManager.AppSettings["smtpUser"];
                string password = ConfigurationManager.AppSettings["smtpPass"];
                string fromEmail = ConfigurationManager.AppSettings["enquiryReciever"].ToString();
                MailMessage message = new MailMessage();
                message.To.Add(Email);
                message.CC.Add(fromEmail);
                message.Bcc.Add("vishal8304@gmail.com");
                message.From = new MailAddress(fromEmail);
                message.Subject = "Desi Utpad : Customer Billing ";
                string textBody = " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#4da6ff'><td><b>Product Name</b></td> <td> <b> Quantity</b> </td><td> <b> Price</b> </td><td> <b>Total</b> </td></tr>";
                foreach (var item in OrderDetails)
                {
                    textBody += "<tr><td>" + item.ProductName + "</td><td> " + item.Quantity + "</td><td> " + item.Price + "</td><td> " + item.FinalPrice + "</td></tr>";
                }

                textBody += "<tr><td> Grand Total</td><td></td><td></td><td> " + TotalAmount + "</td></tr><tr><td> Remarks </td><td></td><td></td></tr></table>";
                message.Body = "Thanks for Purchase with Desi Utpad <br /> Mr/Mrs/Sh/Smt :" + Name + " <br /> on Order Date " + OrderDate + " <br /> Mobile No " + MobileNumber + " <br /> Address " + Address + " <br />." + textBody + "";
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.SubjectEncoding = System.Text.Encoding.Default;
                message.IsBodyHtml = true; 
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "webmail.desiutpad.in";
                smtp.Port = 25;
                //smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(sender, password);
                //smtp.EnableSsl = false;
                smtp.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static bool SendTestEmail(string recipeint, bool IsBodyHtml)
        {
            try
            {
                string sender = ConfigurationManager.AppSettings["smtpUser"];
                string password = ConfigurationManager.AppSettings["smtpPass"];
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sender, "DesiUtpad");
                message.To.Add(recipeint);
                message.Subject = "Hello";
                message.Body = "Test";
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.SubjectEncoding = System.Text.Encoding.Default;
                message.IsBodyHtml = IsBodyHtml;
                //if (!string.IsNullOrEmpty(attachmentPath))
                //{
                //    System.Net.Mail.Attachment attachment;
                //    attachment = new System.Net.Mail.Attachment(attachmentPath);
                //    message.Attachments.Add(attachment);
                //}
                SmtpClient smtp = new SmtpClient();

                smtp.Host = "webmail.desiutpad.in";

                smtp.Port = 25;
                //smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(sender, password);
                smtp.EnableSsl = false; 
                smtp.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}