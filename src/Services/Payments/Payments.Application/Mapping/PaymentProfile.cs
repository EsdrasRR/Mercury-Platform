using AutoMapper;
using Payments.Application.DTOs;
using Payments.Domain.Entities;

namespace Payments.Application.Mapping;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<Payment, PaymentDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
    }
}
