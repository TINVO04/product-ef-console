using ProductEfConsole.Models;
using ProductEfConsole.Repositories;

namespace ProductEfConsole.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task AddProductAsync(Product product)
    {
        await _productRepository.AddAsync(product);
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(product.Id);

        if (existingProduct is null)
        {
            return false;
        }

        await _productRepository.UpdateAsync(product);
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct is null)
        {
            return false;
        }

        await _productRepository.DeleteAsync(id);
        return true;
    }

    public async Task<List<Product>> GetPagedProductsAsync(string? search, int page, int pageSize, string? sortBy)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 5;
        }

        return await _productRepository.GetPagedProductsAsync(search, page, pageSize, sortBy);
    }

}
