using System.ComponentModel.DataAnnotations;

namespace ExampleCom.ProductManagement.Api.Models.Dto;

public class CreateProductRequest
{
    [Required(ErrorMessage = "ProductCode is required")]
    [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$",
        ErrorMessage = "ProductCode must be in format XXXX-XXXX-XXXX-XXXX (uppercase letters and digits only)")]
    public string ProductCode { get; set; } = string.Empty;
}