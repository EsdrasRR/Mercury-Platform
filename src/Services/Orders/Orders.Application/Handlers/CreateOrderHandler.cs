using AutoMapper;
using MediatR;
using Orders.Application.Commands;
using Orders.Application.DTOs;
using Orders.Application.Messaging;
using Orders.Domain.Entities;
using Orders.Infrastructure;
using Shared.Contracts.Events;

namespace Orders.Application.Handlers;

public class CreateOrderHandler(IOrderRepository repository, IMapper mapper, IEventPublisher publisher) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IEventPublisher _publisher = publisher;

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order(request.CustomerId, request.ShippingAddress);

        foreach (var item in request.Items)
        {
            order.AddItem(item.ProductId, item.Quantity, item.Price);
        }

        await _repository.AddAsync(order, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount,
            Items =
            [
                .. order.Items.Select(i =>
                    new Shared.Contracts.Events.OrderItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity  = i.Quantity,
                        Price     = i.Price
                    })
            ]
        };

        _ = Task.Run(() => _publisher.PublishAsync(orderCreatedEvent), cancellationToken);

        var dto = _mapper.Map<OrderDto>(order);
        return dto;
    }
}

