using MediatR;
using Orders.Application.DTOs;

namespace Orders.Application.Commands;

public class CreateOrderCommand : IRequest<OrderDto>
{
    public string CustomerId { get; set; } = string.Empty;

    public string ShippingAddress { get; set; } = string.Empty;

    public List<CreateOrderItemDto> Items { get; set; } = [];
}
