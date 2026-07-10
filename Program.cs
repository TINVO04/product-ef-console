using ProductEfConsole.Data;
using ProductEfConsole.Models;
using ProductEfConsole.Repositories;
using ProductEfConsole.Services;

using var context = new AppDbContext();

IProductRepository productRepository = new ProductRepository(context);
var productService = new ProductService(productRepository);

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

Console.WriteLine("\nDone.");
