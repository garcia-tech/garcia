using Garcia.Application.Contracts.Email;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.Email.Mandrill
{
    public class MandrillEmailService : IMandrillEmailService
    {
        protected MandrillEmailSettings _settings;
        private readonly ILogger _logger;

        public MandrillEmailService(IOptions<MandrillEmailSettings> settings, ILoggerFactory logger)
        {
            _settings = settings.Value;
            _logger = logger.CreateLogger<MandrillEmailService>();
        }

        public async Task SendEmailAsync(string templateName, string recipientEmailAddress, string recipientFullName, string[]? cc = null, string[]? bcc = null, Dictionary<string, string>? parameters = null, IEnumerable<EmailAttachment> attachments = null)
        {
            try
            {
                MandrillApi api = new MandrillApi(_settings.ApiKey);
                var toList = new List<EmailAddress>();
                toList.Add(new EmailAddress(recipientEmailAddress, recipientFullName));

                if (bcc != null && bcc.Any())
                {
                    foreach (var item in bcc)
                    {
                        toList.Add(new EmailAddress(item, recipientFullName, "bcc"));
                    }
                }

                if (cc != null && cc.Any())
                {
                    foreach (var item in cc)
                    {
                        toList.Add(new EmailAddress(item, recipientFullName, "cc"));
                    }
                }

                if (_settings.Bcc != null)
                {
                    toList.AddRange(_settings.Bcc.Split(',').Select(x => new EmailAddress(x, recipientFullName, "bcc")));
                }

                EmailMessage message = new EmailMessage()
                {
                    FromEmail = _settings.SenderEmailAddress,
                    To = toList
                };

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        message.AddGlobalVariable(parameter.Key, parameter.Value);
                    }
                }

                if(attachments?.Any() == true)
                {
                    message.Attachments = attachments;
                }

                var templateRequest = new SendMessageTemplateRequest(message, templateName);
                var result = await api.SendMessageTemplate(templateRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
