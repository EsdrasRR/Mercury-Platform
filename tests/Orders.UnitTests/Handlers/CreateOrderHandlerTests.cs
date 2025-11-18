using AutoMapper;
using Moq;
using Orders.Application.Commands;
using Orders.Application.DTOs;
using Orders.Application.Handlers;
using Orders.Application.Interfaces;
using Orders.Application.Mappings;
using Orders.Application.Messaging;
using Orders.Domain.Entities;
using Shared.Contracts.Events;

namespace Orders.UnitTests.Handlers;

public class CreateOrderHandlerTests
{
	private readonly Mock<IOrderRepository> _repoMock = new();
	private readonly Mock<IEventPublisher> _publisherMock = new();
	private readonly IMapper _mapper;

	public CreateOrderHandlerTests()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile(new OrderMappingProfile()));
		_mapper = config.CreateMapper();
	}

	[Fact]
	public async Task Handle_ShouldCreateOrderAndPublishEvent()
	{
		// Arrange
		var command = new CreateOrderCommand
		{
			CustomerId = "cust-1",
			ShippingAddress = "Rua A, 123",
			Items = 
			{
				new() { ProductId = "prod-1", Quantity = 2, Price = 10m },
				new() { ProductId = "prod-2", Quantity = 1, Price = 5m }
			}
		};

		Order? capturedOrder = null;
		_repoMock.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
			.Callback<Order, CancellationToken>((o, ct) => capturedOrder = o)
			.Returns(Task.CompletedTask);

		_repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

		var handler = new CreateOrderHandler(_repoMock.Object, _mapper, _publisherMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.NotNull(capturedOrder);
		Assert.Equal("cust-1", capturedOrder!.CustomerId);
		Assert.Equal("Rua A, 123", capturedOrder.ShippingAddress);
		Assert.Equal(2, capturedOrder.Items.Count);
		Assert.Equal(25m, capturedOrder.TotalAmount); // 2*10 + 1*5
		_repoMock.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
		_repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		_publisherMock.Verify(p => p.PublishAsync(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
		Assert.Equal(capturedOrder.Id, result.Id);
		Assert.Equal(capturedOrder.TotalAmount, result.TotalAmount);
	}

	[Fact]
	public async Task Handle_ShouldCreateOrderWithNoItems()
	{
		// Arrange
		var command = new CreateOrderCommand
		{
			CustomerId = "cust-2",
			ShippingAddress = "Rua B, 456",
			Items = []
		};

		Order? capturedOrder = null;
		_repoMock.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
			.Callback<Order, CancellationToken>((o, ct) => capturedOrder = o)
			.Returns(Task.CompletedTask);

		_repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

		var handler = new CreateOrderHandler(_repoMock.Object, _mapper, _publisherMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.NotNull(capturedOrder);
		Assert.Equal("cust-2", capturedOrder!.CustomerId);
		Assert.Equal("Rua B, 456", capturedOrder.ShippingAddress);
		Assert.Empty(capturedOrder.Items);
		Assert.Equal(0m, capturedOrder.TotalAmount);
		_repoMock.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
		_repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		_publisherMock.Verify(p => p.PublishAsync(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
		Assert.Equal(capturedOrder.Id, result.Id);
	}

	[Fact]
	public async Task Handle_ShouldPublishEventWithCorrectData()
	{
		// Arrange
		var command = new CreateOrderCommand
		{
			CustomerId = "cust-3",
			ShippingAddress = "Rua C, 789",
			Items =
			[
				new() { ProductId = "prod-3", Quantity = 3, Price = 15m }
			]
		};

		Order? capturedOrder = null;
		_repoMock.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
			.Callback<Order, CancellationToken>((o, ct) => capturedOrder = o)
			.Returns(Task.CompletedTask);

		_repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

		OrderCreatedEvent? publishedEvent = null;
		_publisherMock.Setup(p => p.PublishAsync(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>()))
			.Callback<OrderCreatedEvent, CancellationToken>((e, ct) => publishedEvent = e)
			.Returns(Task.CompletedTask);

		var handler = new CreateOrderHandler(_repoMock.Object, _mapper, _publisherMock.Object);

		// Act
		await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.NotNull(publishedEvent);
		Assert.Equal(capturedOrder!.Id, publishedEvent!.OrderId);
		Assert.Equal("cust-3", publishedEvent.CustomerId);
		Assert.Equal(45m, publishedEvent.TotalAmount); // 3*15
		Assert.Single(publishedEvent.Items);
		Assert.Equal("prod-3", publishedEvent.Items[0].ProductId);
		Assert.Equal(3, publishedEvent.Items[0].Quantity);
		Assert.Equal(15m, publishedEvent.Items[0].Price);
	}
}
