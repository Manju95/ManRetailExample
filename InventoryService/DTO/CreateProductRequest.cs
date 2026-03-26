namespace InventoryService.DTO;

public class CreateProductRequest
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}