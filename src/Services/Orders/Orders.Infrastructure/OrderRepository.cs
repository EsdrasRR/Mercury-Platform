using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;

namespace Orders.Infrastructure;

public class OrderRepository(OrdersDbContext db) : IOrderRepository
{
    private readonly OrdersDbContext _db = db;

    public Task AddAsync(Order order, CancellationToken ct = default)
    {
        _db.Orders.Add(order);
        return Task.CompletedTask;
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
