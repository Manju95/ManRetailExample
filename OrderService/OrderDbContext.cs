using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) {}

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Email)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(o => o.CreatedAt)
                .IsRequired();

            entity.HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ProductId)
                .IsRequired();
            entity.Property(i => i.Quantity)
                .IsRequired();
            
            entity.HasIndex(i => i.OrderId);
        });
    }
}