using InventoryService.DTO;
using InventoryService.Interfaces;
using InventoryService.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryServices _inventoryServices;

    public InventoryController(IInventoryServices inventoryServices)
    {
        _inventoryServices = inventoryServices ?? throw new ArgumentNullException(nameof(inventoryServices));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _inventoryServices.GetAllProducts();
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _inventoryServices.GetProductById(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Quantity = request.Quantity,
            Price = request.Price
        };

        await _inventoryServices.CreateProduct(product);

        return Ok(product);
    }
    
    [HttpPut("{id}/stock")]
    public async Task<IActionResult> UpdateStock(int id, UpdateStockRequest request)
    {
        Product product = new();
        product.Quantity = request.Quantity;
        
        await _inventoryServices.UpdateStock(id, product);

        return NoContent();
    }
}