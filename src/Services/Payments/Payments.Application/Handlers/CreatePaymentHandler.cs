using AutoMapper;
using MediatR;
using Payments.Application.Commands;
using Payments.Application.DTOs;
using Payments.Application.Interfaces;
using Payments.Domain.Entities;

namespace Payments.Application.Handlers;

public class CreatePaymentHandler(IPaymentRepository repo, IMapper mapper) : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    private readonly IPaymentRepository _repo = repo;
    private readonly IMapper _mapper = mapper;

    public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment(request.OrderId, request.Amount, request.Currency);

        await _repo.AddAsync(payment, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PaymentDto>(payment);
        return dto;
    }
}
