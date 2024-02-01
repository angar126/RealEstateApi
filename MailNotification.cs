using Microsoft.Extensions.Options;
using RealEstateApi.Models;
using RealEstateApi.Models.Interfaces;
using System.Net.Mail;
using System.Net;

namespace RealEstateApi
{
    public class MailNotification : INotifier
    {
        readonly MailConfig _mailConf;
        public MailNotification(IOptions<MailConfig> mailConf)
        {
            _mailConf = mailConf.Value;
        }

        public void SendNotification(string toAddress, string subject, string body)
        {
            var FromAddress = new MailAddress(_mailConf.Username, "AIRBNB");
            var ToAddress = new MailAddress(toAddress);

            var smtp = new SmtpClient
            {
                Host = "smtp.ethereal.email",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_mailConf.Username, _mailConf.Password)
            };

            using (var message = new MailMessage(FromAddress, ToAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
