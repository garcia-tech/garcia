using Garcia.Application.Contracts.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Garcia.Infrastructure.Email.SendGrid
{
    public class SendGridEmailService : ISendGridEmailService
    {
        private readonly SendGridEmailSettings _settings;
        private readonly ILogger<SendGridEmailService> _logger;

        public SendGridEmailService(IOptions<SendGridEmailSettings> options, ILogger<SendGridEmailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string content, string subject, bool isBodyHtml = true, string templateId = null, string[] cc = null, string[] bcc = null, Attachment[] attachments = null, params string[] to)
        {
            try
            {
                var client = new SendGridClient(_settings.ApiKey);

                var msg = new SendGridMessage
                {
                    From = new EmailAddress(_settings.SenderEmailAddress),
                    Subject = subject,
                    TemplateId = templateId ?? _settings.TemplateId
                };

                if (attachments?.Any() == true)
                {
                    msg.AddAttachments(attachments);
                }

                if (cc?.Any() == true)
                {
                    var ccs = cc.Select(x => new EmailAddress(x)).ToList();
                    msg.AddCcs(ccs);
                }

                AddBccs(msg, bcc);
                var tos = to.Select(x => new EmailAddress(x)).ToList();
                msg.AddTos(tos);
                Response response;

                if (isBodyHtml)
                {
                    msg.HtmlContent = content;
                    response = await client.SendEmailAsync(msg);
                    await LogSendGridRepsonse(response);
                    return;
                }

                msg.PlainTextContent = content;
                response = await client.SendEmailAsync(msg);
                await LogSendGridRepsonse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        public async Task SendEmailAsync(string templateId = null, string[] cc = null, string[] bcc = null, Attachment[] attachments = null, params string[] to)
        {
            try
            {
                var client = new SendGridClient(_settings.ApiKey);

                var msg = new SendGridMessage
                {
                    From = new EmailAddress(_settings.SenderEmailAddress),
                    TemplateId = templateId ?? _settings.TemplateId
                };

                if (attachments?.Any() == true)
                {
                    msg.AddAttachments(attachments);
                }

                if (cc?.Any() == true)
                {
                    var ccs = cc.Select(x => new EmailAddress(x)).ToList();
                    msg.AddCcs(ccs);
                }

                AddBccs(msg, bcc);
                var tos = to.Select(x => new EmailAddress(x)).ToList();
                msg.AddTos(tos);

                var response = await client.SendEmailAsync(msg);
                await LogSendGridRepsonse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task LogSendGridRepsonse(Response response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var resultBody = await response.Body.ReadAsStringAsync();
                _logger.LogCritical($"SendGrid error occurred: {resultBody}");
            }
        }

        private void AddBccs(SendGridMessage msg, string[] bcc = null)
        {
            var emails = new List<EmailAddress>();

            if (bcc?.Any() == true)
            {
                emails = emails
                    .Union(bcc.Select(x => new EmailAddress(x)).ToList())
                    .ToList();
            }

            if(!string.IsNullOrEmpty(_settings.Bcc))
            {
                var bccs = _settings.Bcc.Split(',').Select(x => new EmailAddress(x));
                emails = emails.Union(bccs).ToList();
            }

            if (emails.Any())
                msg.AddBccs(emails);
        }
    }
}