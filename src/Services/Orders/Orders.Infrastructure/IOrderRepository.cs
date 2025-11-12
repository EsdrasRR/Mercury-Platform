using Orders.Domain.Entities;

namespace Orders.Infrastructure
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order, CancellationToken ct = default);

        Task<Order?> GetAsync(Guid id, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
