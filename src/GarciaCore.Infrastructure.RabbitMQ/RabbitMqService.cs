using System.Text;
using System.Text.Json;
using GarciaCore.Application.RabbitMQ.Contracts.Infrastructure;
using GarciaCore.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GarciaCore.Infrastructure.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;

        public RabbitMqService(RabbitMqConnetionFactory factory)
        {
            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMqConnectionShutdown;
        }

        public void Publish(string queueName, string message, string exchange = "", bool persistent = true)
        {
            var channel = _connection.CreateModel();
            var props = channel.CreateBasicProperties();
            props.Persistent = persistent;
            
            channel.QueueDeclare(queueName, persistent, false, !persistent);

            if(!string.IsNullOrEmpty(exchange))
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Direct);
                channel.QueueBind(queueName, exchange, $"{exchange}.{queueName}");
            }

            channel.BasicPublish(exchange, queueName, props, Encoding.UTF8.GetBytes(message));
            channel.Close();
        }

        public async Task PublishAsync(string queueName, string message, string exchange = "", bool persistent = true)
        {
            var channel = _connection.CreateModel();
            var props = channel.CreateBasicProperties();
            props.Persistent = persistent;

            channel.QueueDeclareNoWait(queueName, persistent, false, !persistent, null);

            if (!string.IsNullOrEmpty(exchange))
            {
                channel.ExchangeDeclareNoWait(exchange, ExchangeType.Direct, arguments: null);
                channel.QueueBindNoWait(queueName, exchange, $"{exchange}.{queueName}", null);
            }

            channel.BasicPublish(exchange, queueName, props, Encoding.UTF8.GetBytes(message));
            channel.Close();
            await Task.CompletedTask;
        }

        public async Task PublishAsync<T>(T message) where T : IMessage
        {
            var queue = typeof(T).Name;
            var channel = _connection.CreateModel();
            var props = channel.CreateBasicProperties();
            props.Persistent = true;
            channel.QueueDeclare(queue, true, false, false);
            channel.BasicPublish("", queue, props, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
            channel.Close();
            await Task.CompletedTask;
        }

        public void Subscribe(string queueName, Action<ReadOnlyMemory<byte>> receiveHandler, Action<Exception, ReadOnlyMemory<byte>> rejectHandler, 
            bool requeueOnReject = false ,bool persistent = true, uint prefetchSize = 0, ushort prefetchCount = 1, bool global = false)
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queueName, persistent, false, !persistent);
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
            channel.BasicConsume(queueName, false, consumer);
        }

        public async Task SubscribeAsync(string queueName, Func<ReadOnlyMemory<byte>, Task> receiveHandler, Func<Exception, ReadOnlyMemory<byte>, Task> rejectHandler, 
            bool requeueOnReject = false ,bool persistent = true, uint prefetchSize = 0, ushort prefetchCount = 1, bool global = false)
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queueName, persistent, false, !persistent);
            channel.BasicQos(prefetchSize, prefetchCount, global);
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    await receiveHandler(ea.Body);
                    channel.BasicAck(ea.DeliveryTag, true);
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    channel.BasicReject(ea.DeliveryTag, requeueOnReject);
                    await rejectHandler(e, ea.Body);
                }
            };

            consumer.Shutdown += OnConsumerShutdownAsync;
            consumer.Registered += OnConsumerRegisteredAsync;
            consumer.Unregistered += OnConsumerUnregisteredAsync;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelledAsync;
            channel.BasicConsume(queueName, false, consumer);
            await Task.CompletedTask;    
        }

        public async Task SubscribeAsync<T>(Func<T, Task> receiveHandler, Func<Exception, string, Task> rejectHandler) where T : IMessage
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(typeof(T).Name, true, false, false);
            channel.BasicQos(0, 1, false);
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (ch, ea) =>
            {
                var messageContent = Encoding.UTF8.GetString(ea.Body.ToArray());
                
                try
                {
                    var model = JsonSerializer.Deserialize<T>(messageContent);
                    await receiveHandler(model);
                    channel.BasicAck(ea.DeliveryTag, true);
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    channel.BasicReject(ea.DeliveryTag, false);
                    await rejectHandler(e, messageContent);
                }
            };

            consumer.Shutdown += OnConsumerShutdownAsync;
            consumer.Registered += OnConsumerRegisteredAsync;
            consumer.Unregistered += OnConsumerUnregisteredAsync;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelledAsync;
            channel.BasicConsume(typeof(T).Name, false, consumer);
            await Task.CompletedTask;
        }

        public async Task DeleteQueue(string queueName, bool ifUnsued, bool ifEmpty)
        {
            var channel = _connection.CreateModel();
            channel.QueueDeleteNoWait(queueName, ifUnsued, ifEmpty);
            channel.Close();
            await Task.CompletedTask;
        }
        public async Task DeleteExchange(string queueName, bool ifUnsued)
        {
            var channel = _connection.CreateModel();
            channel.ExchangeDeleteNoWait(queueName, ifUnsued);
            channel.Close();
            await Task.CompletedTask;
        }

        private async Task OnConsumerConsumerCancelledAsync (object sender, ConsumerEventArgs e) { }
        private async Task OnConsumerUnregisteredAsync (object sender, ConsumerEventArgs e) { }
        private async Task OnConsumerRegisteredAsync (object sender, ConsumerEventArgs e) { }
        private async Task OnConsumerShutdownAsync (object sender, ShutdownEventArgs e) { }
        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMqConnectionShutdown(object sender, ShutdownEventArgs e) { }
    }
}
