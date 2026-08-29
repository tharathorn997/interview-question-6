namespace ExampleCom.ProductManagement.Api.Models.Dto;

public class ProductResponse
{
    public int Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}