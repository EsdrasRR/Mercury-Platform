using MediatR;
using Payments.Application.DTOs;

namespace Payments.Application.Commands;

public record CreatePaymentCommand(Guid OrderId, decimal Amount, string Currency = "AUD") : IRequest<PaymentDto>;
