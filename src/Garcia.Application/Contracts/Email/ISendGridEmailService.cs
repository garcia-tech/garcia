using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace Garcia.Application.Contracts.Email
{
    public interface ISendGridEmailService
    {
        /// <summary>
        /// Provides sending email via <see cref="ISendGridClient"/>
        /// </summary>
        /// <param name="content">The email content.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="isBodyHtml">This is the flag that indicates whether the email content is html or not.</param>
        /// <param name="templateId">The template id on SendGrid.</param>
        /// <param name="cc">Carbon copy.</param>
        /// <param name="bcc">Blind carbon copy.</param>
        /// <param name="attachments">Attachments to be sent.</param>
        /// <param name="to">The target or targets who will receive the email.</param>
        /// <returns></returns>
        Task SendEmailAsync(string content, string subject, bool isBodyHtml = true, string templateId = null, string[] cc = null, string[] bcc = null, Attachment[] attachments = null, params string[] to);
        /// <summary>
        /// Provides sending email via <see cref="ISendGridClient"/>
        /// </summary>
        /// <param name="templateId">The template id on SendGrid.</param>
        /// <param name="cc">Carbon copy.</param>
        /// <param name="bcc">Blind carbon copy.</param>
        /// <param name="attachments">Attachments to be sent.</param>
        /// <param name="to">The target or targets who will receive the email.</param>
        /// <returns></returns>
        Task SendEmailAsync(string templateId = null, string[] cc = null, string[] bcc = null, Attachment[] attachments = null, params string[] to);
    }
}
