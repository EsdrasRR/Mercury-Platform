using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;

namespace Orders.Infrastructure;

public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
	public DbSet<Order> Orders { get; set; } = null!;
	public DbSet<OrderItem> OrderItems { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		var order = modelBuilder.Entity<Order>();
		order.ToTable("orders");
		order.HasKey(o => o.Id);
		order.Property<byte[]>("RowVersion").IsRowVersion();
		order.Property(o => o.Id).ValueGeneratedNever();
		order.Property(o => o.CustomerId).IsRequired().HasMaxLength(100);
		order.Property(o => o.ShippingAddress).IsRequired().HasMaxLength(500);
		order.Property(o => o.Status).IsRequired();
		order.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
		order.Property(o => o.CreatedAt).IsRequired();
		order.Property(o => o.UpdatedAt);
		order.HasIndex(o => o.CustomerId);
		order.HasIndex(o => o.Status);
		
		order.Ignore(o => o.Items);
		order.HasMany<OrderItem>("_items")
			.WithOne()
			.HasForeignKey(oi => oi.OrderId)
			.OnDelete(DeleteBehavior.Cascade);

		var item = modelBuilder.Entity<OrderItem>();
		item.ToTable("order_items");
		item.HasKey(i => i.Id);
		item.Property(i => i.Id).ValueGeneratedNever();
		item.Property(i => i.ProductId).IsRequired().HasMaxLength(100);
		item.Property(i => i.Quantity).IsRequired();
		item.Property(i => i.Price).HasColumnType("decimal(18,2)").IsRequired();
	}
}

