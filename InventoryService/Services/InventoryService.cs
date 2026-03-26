using InventoryService.Interfaces;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Services;

public class InventoryService : IInventoryServices
{
    private readonly InventoryDbContext _context;
    public InventoryService(InventoryDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product?> CreateProduct(Product request)
    {
        var product = new Product
        {
            Name = request.Name,
            Quantity = request.Quantity,
            Price = request.Price
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateStock(int id, Product request)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return null;

        product.Quantity = request.Quantity;

        await _context.SaveChangesAsync();
        
        return product;
    }
}