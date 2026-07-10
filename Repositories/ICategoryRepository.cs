using ProductEfConsole.Models;

namespace ProductEfConsole.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);

    Task<bool> HasProductsAsync(int categoryId);

    Task DeleteAsync(Category category);

    Task<List<Category>> GetAllAsync();

    Task AddAsync(Category category);

    Task UpdateAsync(Category category);

}
