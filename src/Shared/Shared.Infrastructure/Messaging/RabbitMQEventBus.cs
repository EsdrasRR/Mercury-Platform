using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Shared.Contracts.Events;

namespace Shared.Infrastructure.Messaging;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IIntegrationEvent;
    void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IIntegrationEvent;
}

public class RabbitMQEventBus : IEventBus, IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _exchangeName = "mercury_events";

    public RabbitMQEventBus(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Topic, durable: true).GetAwaiter().GetResult();
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IIntegrationEvent
    {
        var eventName = typeof(TEvent).Name;
        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true,
            MessageId = @event.EventId.ToString()
        };

        await _channel.BasicPublishAsync(
            exchange: _exchangeName,
            routingKey: eventName,
            mandatory: false,
            basicProperties: properties,
            body: body);
    }

    public void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IIntegrationEvent
    {
        var eventName = typeof(TEvent).Name;
        var queueName = $"{eventName}_queue";

        _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null).GetAwaiter().GetResult();

        _channel.QueueBindAsync(
            queue: queueName,
            exchange: _exchangeName,
            routingKey: eventName).GetAwaiter().GetResult();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var @event = JsonSerializer.Deserialize<TEvent>(message);

            if (@event != null)
            {
                await handler(@event);
            }

            await _channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer).GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
    }
}
