using Microsoft.EntityFrameworkCore;
using ProductEfConsole.Data;
using ProductEfConsole.Models;

namespace ProductEfConsole.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetPagedProductsAsync(string? search, int page, int pageSize, string? sortBy)
    {
        IQueryable<Product> query = _context.Products;

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(product => product.Name.ToLower().Contains(search.ToLower()));
        }

        query = sortBy?.ToLower() switch
        {
            "price" => query.OrderBy(product => product.Price),
            "name" => query.OrderBy(product => product.Name),
            _ => query.OrderBy(product => product.Id)
        };

        query = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<List<Product>> GetProductsWithCategoryAsync()
    {
        return await _context.Products
            .Include(product => product.Category)
            .ToListAsync();
    }


}
