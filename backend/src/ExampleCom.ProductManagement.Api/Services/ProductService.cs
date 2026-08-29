using Microsoft.EntityFrameworkCore;
using ExampleCom.ProductManagement.Api.Data;
using ExampleCom.ProductManagement.Api.Models;

namespace ExampleCom.ProductManagement.Api.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.OrderBy(p => p.Id).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(string productCode)
    {
        var product = new Product
        {
            ProductCode = productCode.ToUpper(),
            CreatedAt = DateTime.UtcNow
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string productCode)
    {
        return await _context.Products.AnyAsync(p => p.ProductCode == productCode.ToUpper());
    }
}