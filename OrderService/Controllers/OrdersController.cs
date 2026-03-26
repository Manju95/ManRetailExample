using Microsoft.AspNetCore.Mvc;
using OrderService.DTO;
using OrderService.Interfaces;
using OrderService.Models;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService ?? throw new ArgumentNullException(nameof(ordersService));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _ordersService.GetOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var order = await _ordersService.GetOrderAsync(id);

        if (order == null)
            return NotFound();

        return Ok(order);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request)
    {
        var order = new Order
        {
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            Items = request.Items
        };

        await _ordersService.CreateOrderAsync(order);

        return Ok(order);
    }
}