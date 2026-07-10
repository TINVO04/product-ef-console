using ProductEfConsole.Repositories;
using ProductEfConsole.Models;

namespace ProductEfConsole.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDeleteResult> DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            return CategoryDeleteResult.NotFound;
        }

        var hasProducts = await _categoryRepository.HasProductsAsync(id);

        if (hasProducts)
        {
            return CategoryDeleteResult.HasProducts;
        }

        await _categoryRepository.DeleteAsync(category);
        return CategoryDeleteResult.Success;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _categoryRepository.AddAsync(category);
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        var existingCategory =
            await _categoryRepository.GetByIdAsync(category.Id);

        if (existingCategory is null)
        {
            return false;
        }

        existingCategory.Name = category.Name;

        await _categoryRepository.UpdateAsync(existingCategory);
        return true;
    }
}
