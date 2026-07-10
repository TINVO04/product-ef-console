using ProductEfConsole.Models;
using ProductEfConsole.Services;

namespace ProductEfConsole;

public class ConsoleApp
{
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;

    public ConsoleApp(
        ProductService productService,
        CategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task RunAsync()
    {
        var isRunning = true;

        while (isRunning)
        {
            PrintMainMenu();

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await RunCategoryMenuAsync();
                    break;
                case "2":
                    await RunProductMenuAsync();
                    break;
                case "3":
                    await SearchProductsAsync();
                    break;
                case "0":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            if (isRunning)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        Console.WriteLine("Application closed.");
    }

    private async Task RunCategoryMenuAsync()
    {
        var returnToMainMenu = false;

        while (!returnToMainMenu)
        {
            Console.Clear();
            PrintCategoryMenu();

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ListCategoriesAsync();
                    break;
                case "2":
                    await AddCategoryAsync();
                    break;
                case "3":
                    await UpdateCategoryAsync();
                    break;
                case "4":
                    await DeleteCategoryAsync();
                    break;
                case "0":
                    returnToMainMenu = true;
                    continue;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    private async Task ListCategoriesAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        Console.WriteLine("\n=== Category List ===");

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories found.");
            return;
        }

        foreach (var category in categories)
        {
            Console.WriteLine(
                $"Id: {category.Id}, Name: {category.Name}");
        }
    }

    private async Task AddCategoryAsync()
    {
        Console.Write("Category name: ");
        var name = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Category name is required.");
            return;
        }

        var categories = await _categoryService.GetAllCategoriesAsync();

        var categoryExists = categories.Any(
            category => category.Name.Equals(
                name,
                StringComparison.OrdinalIgnoreCase));

        if (categoryExists)
        {
            Console.WriteLine("Category name already exists.");
            return;
        }

        var category = new Category
        {
            Name = name
        };

        await _categoryService.AddCategoryAsync(category);

        Console.WriteLine(
            $"Added category successfully. Id: {category.Id}");
    }

    private async Task UpdateCategoryAsync()
    {
        Console.Write("Category id: ");

        if (!int.TryParse(Console.ReadLine(), out var id) || id < 1)
        {
            Console.WriteLine("Category id must be a positive number.");
            return;
        }

        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category is null)
        {
            Console.WriteLine("Category not found.");
            return;
        }

        Console.Write($"New name ({category.Name}): ");
        var newName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(newName))
        {
            Console.WriteLine("Category name is required.");
            return;
        }

        var categories = await _categoryService.GetAllCategoriesAsync();
        var duplicateNameExists = categories.Any(
            existingCategory =>
                existingCategory.Id != id &&
                existingCategory.Name.Equals(
                    newName,
                    StringComparison.OrdinalIgnoreCase));

        if (duplicateNameExists)
        {
            Console.WriteLine("Category name already exists.");
            return;
        }

        category.Name = newName;

        var updateSucceeded =
            await _categoryService.UpdateCategoryAsync(category);

        Console.WriteLine(updateSucceeded
            ? "Updated category successfully."
            : "Category not found for update.");
    }

    private async Task DeleteCategoryAsync()
    {
        Console.Write("Category id: ");

        if (!int.TryParse(Console.ReadLine(), out var id) || id < 1)
        {
            Console.WriteLine("Category id must be a positive number.");
            return;
        }

        var result = await _categoryService.DeleteCategoryAsync(id);

        var message = result switch
        {
            CategoryDeleteResult.Success =>
                "Deleted category successfully.",
            CategoryDeleteResult.NotFound =>
                "Category not found.",
            CategoryDeleteResult.HasProducts =>
                "Cannot delete category because it still has products.",
            _ => "Unknown delete result."
        };

        Console.WriteLine(message);
    }

    private async Task RunProductMenuAsync()
    {
        var returnToMainMenu = false;

        while (!returnToMainMenu)
        {
            Console.Clear();
            PrintProductMenu();

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ListProductsAsync();
                    break;
                case "2":
                    await AddProductAsync();
                    break;
                case "3":
                    await UpdateProductAsync();
                    break;
                case "4":
                    await DeleteProductAsync();
                    break;
                case "0":
                    returnToMainMenu = true;
                    continue;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    private async Task ListProductsAsync()
    {
        var products = await _productService.GetProductsWithCategoryAsync();

        Console.WriteLine("\n=== Product List ===");

        if (products.Count == 0)
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var product in products)
        {
            var categoryName = product.Category?.Name ?? "No category";

            Console.WriteLine(
                $"Id: {product.Id}, Name: {product.Name}, " +
                $"Price: {product.Price}, Quantity: {product.Quantity}, " +
                $"Category: {categoryName}");
        }
    }

    private async Task AddProductAsync()
    {
        Console.Write("Product name: ");
        var name = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Product name is required.");
            return;
        }

        Console.Write("Price: ");

        if (!decimal.TryParse(Console.ReadLine(), out var price) || price < 0)
        {
            Console.WriteLine("Price must be a non-negative number.");
            return;
        }

        Console.Write("Quantity: ");

        if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity < 0)
        {
            Console.WriteLine("Quantity must be a non-negative number.");
            return;
        }

        var categoryId = await ReadOptionalCategoryIdAsync();

        if (categoryId == int.MinValue)
        {
            return;
        }

        var product = new Product
        {
            Name = name,
            Price = price,
            Quantity = quantity,
            CategoryId = categoryId
        };

        await _productService.AddProductAsync(product);

        Console.WriteLine(
            $"Added product successfully. Id: {product.Id}");
    }

    private async Task UpdateProductAsync()
    {
        Console.Write("Product id: ");

        if (!int.TryParse(Console.ReadLine(), out var id) || id < 1)
        {
            Console.WriteLine("Product id must be a positive number.");
            return;
        }

        var product = await _productService.GetProductByIdAsync(id);

        if (product is null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        Console.Write($"New name ({product.Name}): ");
        var nameInput = Console.ReadLine()?.Trim();

        if (!string.IsNullOrWhiteSpace(nameInput))
        {
            product.Name = nameInput;
        }

        Console.Write($"New price ({product.Price}): ");
        var priceInput = Console.ReadLine()?.Trim();

        if (!string.IsNullOrWhiteSpace(priceInput))
        {
            if (!decimal.TryParse(priceInput, out var price) || price < 0)
            {
                Console.WriteLine("Price must be a non-negative number.");
                return;
            }

            product.Price = price;
        }

        Console.Write($"New quantity ({product.Quantity}): ");
        var quantityInput = Console.ReadLine()?.Trim();

        if (!string.IsNullOrWhiteSpace(quantityInput))
        {
            if (!int.TryParse(quantityInput, out var quantity) || quantity < 0)
            {
                Console.WriteLine("Quantity must be a non-negative number.");
                return;
            }

            product.Quantity = quantity;
        }

        Console.WriteLine(
            "Enter a category id, leave blank to keep the current category, " +
            "or enter 0 to remove the category.");
        Console.Write("Category id: ");
        var categoryInput = Console.ReadLine()?.Trim();

        if (!string.IsNullOrWhiteSpace(categoryInput))
        {
            if (!int.TryParse(categoryInput, out var categoryId) || categoryId < 0)
            {
                Console.WriteLine(
                    "Category id must be zero or a positive number.");
                return;
            }

            if (categoryId == 0)
            {
                product.CategoryId = null;
            }
            else
            {
                var category =
                    await _categoryService.GetCategoryByIdAsync(categoryId);

                if (category is null)
                {
                    Console.WriteLine("Category not found.");
                    return;
                }

                product.CategoryId = categoryId;
            }
        }

        var updateSucceeded =
            await _productService.UpdateProductAsync(product);

        Console.WriteLine(updateSucceeded
            ? "Updated product successfully."
            : "Product not found for update.");
    }

    private async Task DeleteProductAsync()
    {
        Console.Write("Product id: ");

        if (!int.TryParse(Console.ReadLine(), out var id) || id < 1)
        {
            Console.WriteLine("Product id must be a positive number.");
            return;
        }

        var deleteSucceeded =
            await _productService.DeleteProductAsync(id);

        Console.WriteLine(deleteSucceeded
            ? "Deleted product successfully."
            : "Product not found for delete.");
    }

    private async Task<int?> ReadOptionalCategoryIdAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        Console.WriteLine("\nAvailable categories:");

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories found. Product will have no category.");
            return null;
        }

        foreach (var category in categories)
        {
            Console.WriteLine($"Id: {category.Id}, Name: {category.Name}");
        }

        Console.Write("Category id (leave blank for no category): ");
        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        if (!int.TryParse(input, out var categoryId) || categoryId < 1)
        {
            Console.WriteLine("Category id must be a positive number.");
            return int.MinValue;
        }

        var selectedCategory =
            await _categoryService.GetCategoryByIdAsync(categoryId);

        if (selectedCategory is null)
        {
            Console.WriteLine("Category not found.");
            return int.MinValue;
        }

        return categoryId;
    }

    private async Task SearchProductsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Product Search ===");

        Console.Write("Search text (leave blank for all products): ");
        var search = Console.ReadLine()?.Trim();

        Console.Write("Page: ");
        var pageInput = Console.ReadLine()?.Trim();
        var page = 1;

        if (!string.IsNullOrWhiteSpace(pageInput) &&
            (!int.TryParse(pageInput, out page) || page < 1))
        {
            Console.WriteLine("Page must be a positive number.");
            return;
        }

        Console.Write("Page size: ");
        var pageSizeInput = Console.ReadLine()?.Trim();
        var pageSize = 5;

        if (!string.IsNullOrWhiteSpace(pageSizeInput) &&
            (!int.TryParse(pageSizeInput, out pageSize) || pageSize < 1))
        {
            Console.WriteLine("Page size must be a positive number.");
            return;
        }

        Console.Write("Sort by (id/name/price): ");
        var sortBy = Console.ReadLine()?.Trim().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(sortBy) &&
            sortBy is not "id" and not "name" and not "price")
        {
            Console.WriteLine("Sort field must be id, name, or price.");
            return;
        }

        var products = await _productService.GetPagedProductsAsync(
            search,
            page,
            pageSize,
            sortBy);

        Console.WriteLine("\n=== Search Results ===");
        Console.WriteLine(
            $"Search: {search ?? "All"}, Page: {page}, " +
            $"Page size: {pageSize}, Sort by: {sortBy ?? "id"}");

        if (products.Count == 0)
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine(
                $"Id: {product.Id}, Name: {product.Name}, " +
                $"Price: {product.Price}, Quantity: {product.Quantity}");
        }
    }

    private static void PrintProductMenu()
    {
        Console.WriteLine("=== Product Management ===");
        Console.WriteLine("1. List Products");
        Console.WriteLine("2. Add Product");
        Console.WriteLine("3. Update Product");
        Console.WriteLine("4. Delete Product");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Choose an option: ");
    }

    private static void PrintCategoryMenu()
    {
        Console.WriteLine("=== Category Management ===");
        Console.WriteLine("1. List Categories");
        Console.WriteLine("2. Add Category");
        Console.WriteLine("3. Update Category");
        Console.WriteLine("4. Delete Category");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Choose an option: ");
    }

    private static void PrintMainMenu()
    {
        Console.WriteLine("=== Product EF Console ===");
        Console.WriteLine("1. Manage Categories");
        Console.WriteLine("2. Manage Products");
        Console.WriteLine("3. Search Products");
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");
    }
}
