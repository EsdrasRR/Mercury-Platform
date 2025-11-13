using AutoMapper;
using Orders.Application.DTOs;
using Orders.Domain.Entities;

namespace Orders.Application.Mappings;

public class OrderMappingProfile : Profile
{
	public OrderMappingProfile()
	{
		CreateMap<Order, OrderDto>()
			.ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
			.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

		CreateMap<OrderItem, OrderItemDto>();
	}
}

