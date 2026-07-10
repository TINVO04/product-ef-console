using ProductEfConsole.Models;

namespace ProductEfConsole.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(int id);

    Task<List<Product>> GetPagedProductsAsync(string? search, int page, int pageSize, string? sortBy);
    Task<List<Product>> GetProductsWithCategoryAsync();
    
}
