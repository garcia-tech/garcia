using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace GaricaCore.Infrastructure.RabbitMQ
{
    public class RabbitMqConnetionFactory : IDisposable
    {
        private readonly RabbitMqSettings _settings;
        private readonly IConnectionFactory _factory;
        private IConnection _connection;
        
        public RabbitMqConnetionFactory(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
            _factory = new ConnectionFactory();
            _factory.Uri = new Uri($"amqp://{_settings.UserName}:{_settings.Password}@" +
                $"{_settings.Host}:{_settings.Port}");
        }

        public RabbitMqConnetionFactory(RabbitMqSettings settings)
        {
            _settings = settings;
            _factory = new ConnectionFactory();
            _factory.Uri = new Uri($"amqp://{_settings.UserName}:{_settings.Password}@" +
                $"{_settings.Host}:{_settings.Port}");
        }

        public IConnection CreateConnection()
        {
            _connection = _factory.CreateConnection();
            return _connection;
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
