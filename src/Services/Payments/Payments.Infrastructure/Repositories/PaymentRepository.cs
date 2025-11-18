using Microsoft.EntityFrameworkCore;
using Payments.Application.Interfaces;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Repositories;

public class PaymentRepository(PaymentsDbContext db) : IPaymentRepository
{
    private readonly PaymentsDbContext _db = db;

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        await _db.Payments.AddAsync(payment, cancellationToken);
    }

    public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _db.Payments.AsNoTracking()
                 .Where(p => p.OrderId == orderId)
                 .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
