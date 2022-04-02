using GarciaCore.Application.Contracts.Email;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Application.Email.Mandrill
{
    public class MandrillEmailService : IEmailService
    {
        protected MandrillEmailSettings _settings;
        private ILogger _logger;

        public MandrillEmailService(IOptions<MandrillEmailSettings> settings, ILoggerFactory logger)
        {
            _settings = settings?.Value;
            _logger = logger.CreateLogger<MandrillEmailService>();
        }

        public async Task SendEmailAsync(string templateName, string recipientEmailAddress, string recipientFullName, string bcc, Dictionary<string, string> parameters)
        {
            try
            {
                MandrillApi api = new MandrillApi(_settings.ApiKey);
                var toList = new List<EmailAddress>();
                toList.Add(new EmailAddress(recipientEmailAddress, recipientFullName));

                if (!string.IsNullOrEmpty(bcc))
                {
                    toList.Add(new EmailAddress(bcc, recipientFullName, "bcc"));
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

                var templateRequest = new SendMessageTemplateRequest(message, templateName);
                var result = await api.SendMessageTemplate(templateRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
