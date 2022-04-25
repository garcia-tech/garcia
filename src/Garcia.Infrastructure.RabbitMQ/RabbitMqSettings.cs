namespace Garcia.Infrastructure.RabbitMQ
{
    public class RabbitMqSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public virtual string GetHostKeyValue() => nameof(Host);
        public virtual string GetPortKeyValue() => nameof(Port);
        public virtual string GetPasswordKeyValue() => nameof(Password);
        public virtual string GetUserNameKeyValue() => nameof(UserName);
    }
}