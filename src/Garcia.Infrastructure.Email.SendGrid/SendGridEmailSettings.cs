namespace Garcia.Infrastructure.Email.SendGrid
{
    public class SendGridEmailSettings
    {
        public string ApiKey { get; set; }
        public string SenderEmailAddress { get; set; }
        public string TemplateId { get; set; }
        public string Bcc { get; set; }
    }
}
