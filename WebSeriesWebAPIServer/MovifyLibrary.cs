using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebSeriesWebAPIServer
{
    public class MovifyLibrary
    {
        public static void SendMail(String subject, String body, String to)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            //ngassetstracking@gmail.com , assets@123
            smtp.Credentials = new System.Net.NetworkCredential("movifywebseries@gmail.com", "movify@1234");
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage();
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            string toaddress = to;
            msg.To.Add(toaddress);
            string fromaddress = "movifywebseries@gmail.com";
            msg.From = new MailAddress(fromaddress, "Movify");
            try
            {
                smtp.Send(msg);
            }
            catch
            {
                throw;
            }
        }
    }
}