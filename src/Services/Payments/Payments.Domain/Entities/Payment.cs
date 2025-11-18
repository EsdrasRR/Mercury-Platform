namespace Payments.Domain.Entities;

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}

public class Payment
{
    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public decimal Amount { get; private set; }

    public string Currency { get; private set; } = "AUD";

    public PaymentStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? ProcessedAt { get; private set; }

    public string? ExternalPaymentId { get; private set; }

    public string? ErrorMessage { get; private set; }

    // EF constructor
    protected Payment() { }

    public Payment(Guid orderId, decimal amount, string currency = "AUD")
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Currency = currency;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkCompleted(string externalId)
    {
        Status = PaymentStatus.Completed;
        ExternalPaymentId = externalId;
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkFailed(string error)
    {
        Status = PaymentStatus.Failed;
        ErrorMessage = error;
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkRefunded()
    {
        Status = PaymentStatus.Refunded;
        ProcessedAt = DateTime.UtcNow;
    }
}
