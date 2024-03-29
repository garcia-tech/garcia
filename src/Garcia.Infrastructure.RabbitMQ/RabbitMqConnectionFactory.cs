﻿using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Garcia.Infrastructure.RabbitMQ
{
    public class RabbitMqConnectionFactory : IDisposable
    {
        private readonly RabbitMqSettings _settings;
        private readonly IConnectionFactory _factory;
        private IConnection _connection;

        public RabbitMqConnectionFactory(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
            _factory = new ConnectionFactory();
            _factory.Uri = new Uri($"amqp://{_settings.UserName}:{_settings.Password}@" +
                $"{_settings.Host}:{_settings.Port}");
        }

        public RabbitMqConnectionFactory(RabbitMqSettings settings)
        {
            _settings = settings;
            _factory = new ConnectionFactory();
            _factory.Uri = new Uri($"amqp://{_settings.UserName}:{_settings.Password}@" +
                $"{_settings.Host}:{_settings.Port}");
        }

        public IConnection CreateConnection()
        {
            try
            {
                _connection = _factory.CreateConnection();
                return _connection;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
