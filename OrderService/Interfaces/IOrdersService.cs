using OrderService.Models;

namespace OrderService.Interfaces;

public interface IOrdersService
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> GetOrderAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
}