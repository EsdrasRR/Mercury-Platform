using Orders.Domain.Entities;

namespace Orders.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order, CancellationToken ct = default);

        Task<Order?> GetAsync(Guid id, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
