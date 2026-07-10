using ProductEfConsole;
using ProductEfConsole.Data;
using ProductEfConsole.Repositories;
using ProductEfConsole.Services;

using var context = new AppDbContext();

IProductRepository productRepository =
    new ProductRepository(context);

ICategoryRepository categoryRepository =
    new CategoryRepository(context);

var productService = new ProductService(productRepository);
var categoryService = new CategoryService(categoryRepository);

var app = new ConsoleApp(productService, categoryService);

await app.RunAsync();
