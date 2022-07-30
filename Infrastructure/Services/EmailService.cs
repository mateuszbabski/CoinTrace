using Application.DTOs.Email;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Settings;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using MimeKit.Text;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.EmailFrom);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };


                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);

                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch(Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }
    }
}


