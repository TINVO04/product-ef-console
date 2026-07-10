using ProductEfConsole.Data;
using ProductEfConsole.Models;
using ProductEfConsole.Repositories;
using ProductEfConsole.Services;

using var context = new AppDbContext();

IProductRepository productRepository = new ProductRepository(context);
var productService = new ProductService(productRepository);

ICategoryRepository categoryRepository = new CategoryRepository(context);
var categoryService = new CategoryService(categoryRepository);

Console.WriteLine("=== Product EF Console - Day 2 CRUD Test ===");

var newProduct = new Product
{
    Name = "Keyboard",
    Price = 250000,
    Quantity = 10
};

await productService.AddProductAsync(newProduct);

Console.WriteLine($"Added product: {newProduct.Name}");

var products = await productService.GetAllProductsAsync();

Console.WriteLine("\nProduct list:");

foreach (var product in products)
{
    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}, CreatedAt: {product.CreatedAt}");
}

var firstProduct = products.FirstOrDefault();

if (firstProduct is not null)
{
    var productDetail = await productService.GetProductByIdAsync(firstProduct.Id);

    if (productDetail is not null)
    {
        Console.WriteLine($"\nFound product by id {firstProduct.Id}: {productDetail.Name}");

        productDetail.Price = 300000;
        productDetail.Quantity = 15;

        var updateResult = await productService.UpdateProductAsync(productDetail);

        Console.WriteLine(updateResult
            ? "Updated product successfully."
            : "Product not found for update.");

        var deleteResult = await productService.DeleteProductAsync(productDetail.Id);

        Console.WriteLine(deleteResult
            ? "Deleted product successfully."
            : "Product not found for delete.");
    }
}

var productsAfterDelete = await productService.GetAllProductsAsync();

Console.WriteLine("\nProduct list after delete:");

foreach (var product in productsAfterDelete)
{
    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}, CreatedAt: {product.CreatedAt}");
}

Console.WriteLine("\n=== Day 3 Search / Pagination / Sort Test ===");

var day3Products = new List<Product>
{
    new Product { Name = "Keyboard", Price = 250000, Quantity = 10 },
    new Product { Name = "Mouse", Price = 150000, Quantity = 20 },
    new Product { Name = "Monitor", Price = 2500000, Quantity = 5 },
    new Product { Name = "Keycap Set", Price = 350000, Quantity = 8 },
    new Product { Name = "Laptop Stand", Price = 450000, Quantity = 12 },
    new Product { Name = "USB Cable", Price = 80000, Quantity = 30 }
};

foreach (var product in day3Products)
{
    await productService.AddProductAsync(product);
}

var pagedProducts = await productService.GetPagedProductsAsync(
    search: "key",
    page: 1,
    pageSize: 5,
    sortBy: "price");

Console.WriteLine("\nSearch = key, Page = 1, PageSize = 5, SortBy = price:");

foreach (var product in pagedProducts)
{
    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}");
}

var sortedByNameProducts = await productService.GetPagedProductsAsync(
    search: null,
    page: 1,
    pageSize: 5,
    sortBy: "name");

Console.WriteLine("\nPage = 1, PageSize = 5, SortBy = name:");

foreach (var product in sortedByNameProducts)
{
    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}");
}

Console.WriteLine("\n=== Day 4 Category / Product Include Test ===");

var accessoryCategory = new Category
{
    Name = "Accessory"
};

context.Categories.Add(accessoryCategory);
await context.SaveChangesAsync();

var day4Product = new Product
{
    Name = "Mechanical Keyboard",
    Price = 1200000,
    Quantity = 3,
    CategoryId = accessoryCategory.Id
};

await productService.AddProductAsync(day4Product);

var productsWithCategory = await productService.GetProductsWithCategoryAsync();

Console.WriteLine("\nProduct list with category:");

foreach (var product in productsWithCategory)
{
    var categoryName = product.Category?.Name ?? "No category";

    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Category: {categoryName}, Price: {product.Price}, Quantity: {product.Quantity}");
}

Console.WriteLine("\n=== Day 4 Delete Category Validation Test ===");

var categoryDeleteResult = await categoryService.DeleteCategoryAsync(accessoryCategory.Id);

var categoryDeleteMessage = categoryDeleteResult switch
{
    CategoryDeleteResult.Success => "Deleted category successfully.",
    CategoryDeleteResult.NotFound => "Category not found.",
    CategoryDeleteResult.HasProducts => "Cannot delete category because it still has products.",
    _ => "Unknown delete result."
};

Console.WriteLine(categoryDeleteMessage);

Console.WriteLine("\nDone.");
