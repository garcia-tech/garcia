using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Email
{
    public interface IEmailService
    {
        /// <summary>
        /// Provides sending email via <see cref="SmtpClient"/>
        /// </summary>
        /// <param name="to">The target who will receive the email.</param>
        /// <param name="content">The email content.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="isBodyHtml">This is the flag that indicates whether the email content is html or not.</param>
        /// <param name="attachmentPath">The path of the attachment to be sent.</param>
        /// <param name="attachmentName">The name of the attachment to be sent.</param>
        /// <param name="cc">Carbon copy.</param>
        /// <param name="bcc">Blind carbon copy.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        Task SendEmailAsync(string to, string content, string subject, bool isBodyHtml = true, string attachmentPath = null, string attachmentName = null, string[] cc = null, string[] bcc = null);
    }
}
