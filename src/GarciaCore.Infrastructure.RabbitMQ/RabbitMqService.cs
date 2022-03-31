using System.Text;
using GarciaCore.Application.RabbitMQ.Contracts.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GaricaCore.Infrastructure.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;

        public RabbitMqService(RabbitMqConnetionFactory factory)
        {
            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMqConnectionShutdown;
        }

        public void Publish(string channelName, string message, string exchange = "", bool persistent = true, bool noWait = false)
        {
            var channel = _connection.CreateModel();
            var props = channel.CreateBasicProperties();
            props.Persistent = persistent;

            IDictionary<string, object> arguments = new Dictionary<string, object>
            {
                { "nowait", noWait }
            };
            
            channel.QueueDeclare(channelName, persistent, false, !persistent, arguments);

            if(!string.IsNullOrEmpty(exchange))
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Direct, arguments: arguments);
                channel.QueueBind(channelName, exchange, $"{exchange}.{channelName}", arguments);
            }

            channel.BasicPublish(exchange, channelName, props, Encoding.UTF8.GetBytes(message));
            channel.Close();
        }

        public void Subscribe(string channelName, Action<ReadOnlyMemory<byte>> receiveHandler, Action<Exception, ReadOnlyMemory<byte>> rejectHandler, 
            bool requeueOnReject = false ,bool persistent = true, uint prefetchSize = 0, ushort prefetchCount = 1, bool global = false)
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(channelName, persistent, false, !persistent);
            channel.BasicQos(prefetchSize, prefetchCount, global);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ch, ea) =>
            {
                try
                {
                    receiveHandler(ea.Body);
                    channel.BasicAck(ea.DeliveryTag, true);
                }
                catch (Exception e)
                {
                    channel.BasicReject(ea.DeliveryTag, requeueOnReject);
                    rejectHandler(e, ea.Body);
                }
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;
            channel.BasicConsume(channelName, false, consumer);
        }

        public void Subscribe(string channelName, Func<ReadOnlyMemory<byte>, Task> receiveHandler, Func<Exception, ReadOnlyMemory<byte>, Task> rejectHandler, 
            bool requeueOnReject = false ,bool persistent = true, uint prefetchSize = 0, ushort prefetchCount = 1, bool global = false)
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(channelName, persistent, false, !persistent);
            channel.BasicQos(prefetchSize, prefetchCount, global);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    await receiveHandler(ea.Body);
                    channel.BasicAck(ea.DeliveryTag, true);
                }
                catch (Exception e)
                {
                    channel.BasicReject(ea.DeliveryTag, requeueOnReject);
                    await rejectHandler(e, ea.Body);
                }
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;
            channel.BasicConsume(channelName, false, consumer);
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMqConnectionShutdown(object sender, ShutdownEventArgs e) { }
    }
}
