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
                    Console.WriteLine("Category management is coming next.");
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
        await Task.CompletedTask;
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
