﻿using Garcia.Application.Contracts.Marketing;
using MailChimp.Net;
using MailChimp.Net.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.Marketing.MailChimp
{
    public class MailChimpMarketingService : IMarketingService
    {
        protected MailChimpMarketingSettings _settings;
        private readonly ILogger _logger;
        private readonly MailChimpManager _mailChimpManager;

        public MailChimpMarketingService(IOptions<MailChimpMarketingSettings> settings, ILoggerFactory logger)
        {
            _settings = settings?.Value;
            _logger = logger.CreateLogger<MailChimpMarketingService>();
            _mailChimpManager = new MailChimpManager(_settings.ApiKey);
        }

        public async Task CreateContactAsync(string email, string name, string surname)
        {
            try
            {
                var listId = _settings.AudienceListId;
                var member = new Member { EmailAddress = email, StatusIfNew = Status.Subscribed };
                member.MergeFields.Add("FNAME", name);
                member.MergeFields.Add("LNAME", surname);
                await _mailChimpManager.Members.AddOrUpdateAsync(listId, member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
