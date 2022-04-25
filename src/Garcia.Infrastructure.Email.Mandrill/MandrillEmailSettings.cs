namespace Garcia.Infrastructure.Email.Mandrill
{
    public class MandrillEmailSettings
    {
        public string ApiKey { get; set; }
        public string SenderEmailAddress { get; set; }
        public string Bcc { get; set; }
    }
}
