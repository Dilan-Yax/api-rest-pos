using ApiRestPos.Domain.IRepositories;
using ApiRestPos.Domain.Models;
using ApiRestPos.Dtos;
using ApiRestPos.Services;
using Moq;

namespace ApiRestPos.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _service = new ProductService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Product>
            {
                new() { Id = 1, Name = "Laptop", Price = 1000m, Stock = 5 },
                new() { Id = 2, Name = "Mouse", Price = 50m, Stock = 10 }
            });

        var result = (await _service.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, dto => Assert.IsType<ProductDto>(dto));
        Assert.Equal("Laptop", result[0].Name);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDto()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Product { Id = 1, Name = "Laptop", Price = 1000m });

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Laptop", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Product?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_AddsAndReturnsDto()
    {
        var dto = new CreateProductDto { Name = "Teclado", Price = 250m, Stock = 8, Category = "Accesorios" };

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => { p.Id = 10; return p; });

        var result = await _service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(10, result.Id);
        Assert.Equal("Teclado", result.Name);
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Product>(p => p.Name == "Teclado")), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CreateAsync_EmptyName_ThrowsArgumentException(string? name)
    {
        var dto = new CreateProductDto { Name = name ?? string.Empty, Price = 100m, Stock = 1 };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_NegativePrice_ThrowsArgumentException()
    {
        var dto = new CreateProductDto { Name = "X", Price = -1m, Stock = 1 };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_NegativeStock_ThrowsArgumentException()
    {
        var dto = new CreateProductDto { Name = "X", Price = 1m, Stock = -1 };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_ReturnsUpdatedDto()
    {
        var dto = new UpdateProductDto { Name = "Laptop Pro", Price = 1500m, Stock = 3, IsActive = false };

        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => p);

        var result = await _service.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Laptop Pro", result.Name);
        Assert.False(result.IsActive);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsNull()
    {
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product?)null);

        var result = await _service.UpdateAsync(999, new UpdateProductDto { Name = "X", Price = 1m, Stock = 1 });

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_DelegatesToRepository()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteAsync(1);

        Assert.True(result);
        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}