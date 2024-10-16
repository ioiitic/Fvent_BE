using Fvent.Repository.UOW;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Services.Imp
{
    public class EmailService(IUnitOfWork uOW, IConfiguration _configuration) : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient
            {
                Host = _configuration["Smtp:Host"],
                Port = int.Parse(_configuration["Smtp:Port"]),
                EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"]),
                Credentials = new NetworkCredential(
                    _configuration["Smtp:Username"],
                    _configuration["Smtp:Password"]
                )
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"], "Fvent"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }

}
