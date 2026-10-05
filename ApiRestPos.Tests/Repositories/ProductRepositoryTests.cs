using ApiRestPos.Domain.Models;
using ApiRestPos.Persistence.Context;
using ApiRestPos.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiRestPos.Tests.Repositories;

public class ProductRepositoryTests
{
    private static PosDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PosDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PosDbContext(options);
    }

    private static async Task SeedAsync(PosDbContext context, params Product[] products)
    {
        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts()
    {
        using var context = CreateContext();
        await SeedAsync(context,
            new Product { Id = 1, Name = "Laptop", Price = 1000m },
            new Product { Id = 2, Name = "Mouse", Price = 50m });

        var repository = new ProductRepository(context);
        var products = (await repository.GetAllAsync()).ToList();

        Assert.Equal(2, products.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsProduct()
    {
        using var context = CreateContext();
        await SeedAsync(context, new Product { Id = 1, Name = "Laptop", Price = 1000m });

        var repository = new ProductRepository(context);
        var product = await repository.GetByIdAsync(1);

        Assert.NotNull(product);
        Assert.Equal("Laptop", product.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        using var context = CreateContext();
        var repository = new ProductRepository(context);

        var product = await repository.GetByIdAsync(999);

        Assert.Null(product);
    }

    [Fact]
    public async Task AddAsync_ReturnsProductWithGeneratedId()
    {
        using var context = CreateContext();
        var repository = new ProductRepository(context);

        var product = new Product { Name = "Teclado", Price = 250m };
        var added = await repository.AddAsync(product);

        Assert.NotEqual(0, added.Id);
        Assert.True(await repository.ExistsAsync(added.Id));
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_UpdatesAndReturnsProduct()
    {
        using var context = CreateContext();
        await SeedAsync(context, new Product { Id = 1, Name = "Laptop", Price = 1000m, IsActive = true });

        var repository = new ProductRepository(context);
        var updated = await repository.UpdateAsync(new Product
        {
            Id = 1,
            Name = "Laptop Pro",
            Price = 1500m,
            IsActive = false
        });

        Assert.NotNull(updated);
        Assert.Equal("Laptop Pro", updated.Name);
        Assert.Equal(1500m, updated.Price);
        Assert.False(updated.IsActive);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsNull()
    {
        using var context = CreateContext();
        var repository = new ProductRepository(context);

        var updated = await repository.UpdateAsync(new Product { Id = 999, Name = "X" });

        Assert.Null(updated);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_ReturnsTrueAndDeletes()
    {
        using var context = CreateContext();
        await SeedAsync(context, new Product { Id = 1, Name = "Laptop" });

        var repository = new ProductRepository(context);
        var deleted = await repository.DeleteAsync(1);

        Assert.True(deleted);
        Assert.False(await repository.ExistsAsync(1));
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        using var context = CreateContext();
        var repository = new ProductRepository(context);

        var deleted = await repository.DeleteAsync(999);

        Assert.False(deleted);
    }
}