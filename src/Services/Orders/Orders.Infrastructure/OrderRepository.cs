using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Infrastructure;

public class OrderRepository : IOrderRepository
{
	private readonly OrdersDbContext _db;
	public OrderRepository(OrdersDbContext db) => _db = db;

	public async Task AddAsync(Order order, CancellationToken ct = default)
	{
		_db.Orders.Add(order);
	}

	public async Task<Order?> GetAsync(Guid id, CancellationToken ct = default)
	{
		return await _db.Orders
			.Include(o => o.Items)
			.FirstOrDefaultAsync(o => o.Id == id, ct);
	}

	public Task SaveChangesAsync(CancellationToken ct = default)
		=> _db.SaveChangesAsync(ct);
}
