using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string content, string subject, bool isBodyHtml = true, string attachmentPath = null, string attachmentName = null, string[] cc = null, string[] bcc = null);
    }
}
