using System.Net.Mail;

namespace Garcia.Infrastructure.Email
{
    public class EmailConfigurations
    {
        public SmtpConfigurations SmtpConfigurations { get; set; }
        public EmailCredentials Credentials { get; set; }
    }

    public class EmailCredentials
    {
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
        public string DisplayName { get; set; }
        public string EmailDomain { get; set; }
    }

    public class SmtpConfigurations
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;
        public bool UseDefaultCredentials { get; set; }
    }
}
