using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NetCoreIdentityExample.Helpers.MailHelper
{
    public class MailHelper
    {
        private readonly string _mailAdress = "recaicingz@gmail.com";
        private readonly string _mailPassword = "password";
        private readonly string _smptHost = "smtp.gmail.com";
        public void MailSend(string to, string from, string subject, string body)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.To.Add(to);
                    mail.From = new MailAddress(from);
                    mail.Subject = subject;
                    mail.Body = body;

                    using (var client = new SmtpClient(_smptHost))
                    {
                        client.Port = 587;
                        client.Credentials = new NetworkCredential(_mailAdress, _mailPassword);
                        client.EnableSsl = true;
                        client.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
