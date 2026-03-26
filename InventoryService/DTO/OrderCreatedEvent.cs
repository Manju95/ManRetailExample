using InventoryService.Models;

namespace InventoryService.DTO;

public class OrderCreatedEvent
{
    public string OrderId { get; set; }
    public List<OrderItem> Items { get; set; }
    public DateTime CreatedAt { get; set; }
}