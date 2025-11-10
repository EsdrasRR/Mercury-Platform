namespace Shared.Contracts.Events;

public class OrderCreatedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class InventoryReservedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool Success { get; set; }
}

public class PaymentProcessedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
    public decimal Amount { get; set; }
    public bool Success { get; set; }
    public string? TransactionId { get; set; }
}

public class CheckoutCompletedEvent : IntegrationEvent
{
    public Guid CheckoutId { get; set; }
    public Guid OrderId { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}
