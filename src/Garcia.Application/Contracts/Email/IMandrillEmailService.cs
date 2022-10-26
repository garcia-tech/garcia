using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Models;


namespace Garcia.Application.Contracts.Email
{
    public interface IMandrillEmailService
    {
        /// <summary>
        /// Provides sending email via <see cref="MandrillApi"/>.
        /// </summary>
        /// <param name="templateName">Template name on Mandrill.</param>
        /// <param name="recipientEmailAddress">The target who will receive email.</param>
        /// <param name="recipientFullName">Fullname of the target receiver who will receive email.</param>
        /// <param name="cc">Carbon copy.</param>
        /// <param name="bcc">Blind carbon copy.</param>
        /// <param name="parameters">Custom parameters in the mandrill email template.</param>
        /// <param name="attachments">Attachments.</param>
        /// <returns></returns>
        Task SendEmailAsync(string templateName, string recipientEmailAddress, string recipientFullName, string[] cc = null, string[] bcc = null, Dictionary<string, string> parameters = null, IEnumerable<EmailAttachment> attachments = null);
    }
}
