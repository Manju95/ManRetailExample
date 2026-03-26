namespace OrderService.Models;

public class Order
{
    public int Id { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}