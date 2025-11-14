using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Commands;
using Orders.Infrastructure;

namespace Orders.API;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IMediator mediator) : ControllerBase
{
	private readonly IMediator _mediator = mediator;

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken ct)
	{
		var orderDto = await _mediator.Send(command, ct);
		return CreatedAtAction(nameof(GetById), new { id = orderDto.Id }, orderDto);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById(Guid id, [FromServices] IOrderRepository repo, CancellationToken ct)
	{
		var order = await repo.GetAsync(id, ct);
		return order is null ? NotFound() : Ok(order);
	}
}
