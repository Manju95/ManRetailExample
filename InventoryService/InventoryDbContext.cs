using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    // Optional: Fluent API configurations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Price)
                .HasColumnType("decimal(10,2)");

            entity.Property(p => p.Quantity)
                .IsRequired();
        });
    }
}