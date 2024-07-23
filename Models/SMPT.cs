using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace OnlineBanking_Final.Models
{


    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }









    }
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var msg = new MailMessage(_smtpSettings.UserName, to, subject, body);

            using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                client.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
                client.EnableSsl = _smtpSettings.EnableSsl;
                client.Send(msg);
            }
        }
    }
    //public class EmailService
    //{
    //    private readonly SmtpSettings _smtpSettings;

    //    public EmailService()
    //    {
    //    }

    //    public EmailService(IOptions<SmtpSettings> smtpSettings)
    //    {
    //        _smtpSettings = smtpSettings.Value;
    //    }

    //    public void SendEmail(string to, string subject, string body)
    //    {
    //        var msg = new MailMessage(_smtpSettings.UserName, to, subject, body);

    //        using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
    //        {
    //            client.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
    //            client.EnableSsl = _smtpSettings.EnableSsl;
    //            client.Send(msg);
    //        }
    //    }
    //}

}
