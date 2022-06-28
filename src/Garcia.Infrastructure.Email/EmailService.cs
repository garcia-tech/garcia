using System.Net;
using System.Net.Mail;
using Garcia.Application.Contracts.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfigurations _emailConfigurations;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<EmailConfigurations> options, ILogger<EmailService> logger)
        {
            _emailConfigurations = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string content, string subject, bool isBodyHtml = true, string attachmentPath = null, string attachmentName = null, string[] cc = null, string[] bcc = null)
        {
            try
            {
                var sender = new MailAddress(_emailConfigurations.Credentials.EmailUsername, _emailConfigurations.Credentials.DisplayName ?? _emailConfigurations.Credentials.EmailUsername);
                var receiver = new MailAddress(to);

                var smtp = new SmtpClient
                {
                    Host = _emailConfigurations.SmtpConfigurations.Host,
                    Port = _emailConfigurations.SmtpConfigurations.Port,
                    EnableSsl = _emailConfigurations.SmtpConfigurations.EnableSsl,
                    DeliveryMethod = _emailConfigurations.SmtpConfigurations.DeliveryMethod,
                    UseDefaultCredentials = _emailConfigurations.SmtpConfigurations.UseDefaultCredentials,
                    Credentials = new NetworkCredential(_emailConfigurations.Credentials.EmailUsername, _emailConfigurations.Credentials.EmailPassword, _emailConfigurations.Credentials.EmailDomain)
                };

                var message = new MailMessage(sender, receiver)
                {
                    Subject = subject,
                    IsBodyHtml = isBodyHtml,
                    Body = content,
                };

                if (cc != null)
                {
                    message.CC.Add(string.Join(",", cc));
                }

                if (bcc != null)
                {
                    message.Bcc.Add(string.Join(",", bcc));
                }

                if (!string.IsNullOrEmpty(attachmentName) && !string.IsNullOrEmpty(attachmentPath))
                {
                    Attachment attachment = new Attachment(attachmentPath);
                    attachment.Name = attachmentName;
                    message.Attachments.Add(attachment);
                }

                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}