using OrderService.Models;

namespace OrderService.DTO;

public class CreateOrderRequest
{
    public string Email { get; set; }
    public List<OrderItem> Items { get; set; }
}