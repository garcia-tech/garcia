using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Email
{
    public interface IMandrillEmailService
    {
        Task SendEmailAsync(string templateName, string recipientEmailAddress, string recipientFullName, string bcc, Dictionary<string, string> parameters);
    }
}
