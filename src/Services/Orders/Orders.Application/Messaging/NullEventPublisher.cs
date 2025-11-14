namespace Orders.Application.Messaging;

public class NullEventPublisher : IEventPublisher
{
	public Task PublishAsync<T>(T @event, CancellationToken ct = default) where T : class
		=> Task.CompletedTask;
}

