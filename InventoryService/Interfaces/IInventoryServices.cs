using InventoryService.Models;

namespace InventoryService.Interfaces;

public interface IInventoryServices
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product?> GetProductById(int id);
    Task<Product?> CreateProduct(Product product);
    Task<Product?> UpdateStock(int id, Product product);
}