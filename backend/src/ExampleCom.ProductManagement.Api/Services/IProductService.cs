using Microsoft.EntityFrameworkCore;
using ExampleCom.ProductManagement.Api.Data;
using ExampleCom.ProductManagement.Api.Models;

namespace ExampleCom.ProductManagement.Api.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(string productCode);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(string productCode);
}