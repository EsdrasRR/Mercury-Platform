namespace Orders.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = [];
}

public class OrderItemDto
{
    public Guid Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class CreateOrderDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public List<CreateOrderItemDto> Items { get; set; } = [];
}

public class CreateOrderItemDto
{
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
