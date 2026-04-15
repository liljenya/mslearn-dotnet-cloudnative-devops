using DataEntities;
using Microsoft.EntityFrameworkCore;

namespace Products.Data;

public sealed class ProductDataContext(DbContextOptions<ProductDataContext> options)
    : DbContext(options)
{
    public DbSet<Product> Product { get; set; } = default!;
}

internal static class Extensions
{
    public static async Task CreateDbIfNotExistsAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ProductDataContext>();

        await context.Database.EnsureCreatedAsync();

        await DbInitializer.InitializeAsync(context);
    }
}

internal static class DbInitializer
{
    public static async Task InitializeAsync(ProductDataContext context)
    {
        if (context.Product.Any())
        {
            return;
        }

        List<Product> products =
        [
            new() { Name = "Solar Powered Flashlight", Description = "A fantastic product for outdoor enthusiasts", Price = 19.99m, ImageUrl = "/api/product/product1.png" },
            
        ];

        context.AddRange(products);

        await context.SaveChangesAsync();
    }
}
