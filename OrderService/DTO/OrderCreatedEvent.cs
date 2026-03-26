using OrderService.Models;

namespace OrderService.DTO;

public class OrderCreatedEvent
{
    public string OrderId { get; set; }
    public List<OrderItem> Items { get; set; }
    public DateTime CreatedAt { get; set; }
}