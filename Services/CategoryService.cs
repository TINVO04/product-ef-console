using ProductEfConsole.Repositories;

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
}
