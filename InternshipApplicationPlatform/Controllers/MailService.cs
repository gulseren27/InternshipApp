using System;
using System.Net;
using System.Net.Mail;

namespace InternshipApplicationPlatform.Controllers
{
    public class MailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUserName;
        private readonly string _smtpPassword;

        public MailService(string smtpHost, int smtpPort, string smtpUserName, string smtpPassword)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUserName = smtpUserName;
            _smtpPassword = smtpPassword;
        }

        public void SendMail(string to, string subject, string body)
        {
            try
            {
                using (var message = new MailMessage(_smtpUserName, to))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var client = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        // SSL/TLS kullanmadan önce STARTTLS komutunu göndermek için
                        client.EnableSsl = true;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(_smtpUserName, _smtpPassword);
                        client.TargetName = _smtpHost;
                        client.Port = _smtpPort;
                        client.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda istenilen işlemler yapılabilir
                throw ex;
            }
        }
    }
}
