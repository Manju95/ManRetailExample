using Microsoft.EntityFrameworkCore;
using OrderService.DTO;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Services;

public class OrdersService : IOrdersService
{
    private readonly OrderDbContext _context;
    private readonly IKafkaProducer _kafkaProducer;
    
    public OrdersService(OrderDbContext context, IKafkaProducer kafkaProducer)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
    }
    
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> GetOrderAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<Order> CreateOrderAsync(Order request)
    {
        var order = new Order
        {
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            Items =  request.Items
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        var orderEvent = new OrderCreatedEvent
        {
            OrderId = order.Id.ToString(),
            CreatedAt = order.CreatedAt,
            Items = order.Items
        };

        await _kafkaProducer.PublishAsync<OrderCreatedEvent>(orderEvent);
        
        return order;
    }
}