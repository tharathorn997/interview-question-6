namespace ExampleCom.ProductManagement.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
