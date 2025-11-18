namespace Payments.Application.DTOs;

public class PaymentDto
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "AUD";

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public string? ExternalPaymentId { get; set; }

    public string? ErrorMessage { get; set; }
}
