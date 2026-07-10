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
                    Console.WriteLine("Product management is coming next.");
                    break;
                case "3":
                    Console.WriteLine("Product search is coming next.");
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
