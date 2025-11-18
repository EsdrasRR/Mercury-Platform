using Microsoft.EntityFrameworkCore;
using Payments.Domain.Entities;

namespace Payments.Infrastructure;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var payment = modelBuilder.Entity<Payment>();
        payment.HasKey(p => p.Id);
        payment.Property(p => p.Amount).HasColumnType("decimal(18,2)");
        payment.Property(p => p.Currency).HasMaxLength(10);
        payment.Property(p => p.Status).HasConversion<string>().IsRequired();
        payment.Property(p => p.CreatedAt).IsRequired();
    }
}
