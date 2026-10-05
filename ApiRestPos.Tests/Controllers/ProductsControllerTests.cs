using ApiRestPos.Controllers;
using ApiRestPos.Domain.IServices;
using ApiRestPos.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiRestPos.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _serviceMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _serviceMock = new Mock<IProductService>();
        _controller = new ProductsController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkWithList()
    {
        _serviceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<ProductDto> { new() { Id = 1, Name = "Laptop" } });

        var result = await _controller.GetProducts();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
    }

    [Fact]
    public async Task GetProductById_ExistingId_ReturnsOk()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(new ProductDto { Id = 1, Name = "Laptop" });

        var result = await _controller.GetProductById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal("Laptop", dto.Name);
    }

    [Fact]
    public async Task GetProductById_NonExistingId_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(999))
            .ReturnsAsync((ProductDto?)null);

        var result = await _controller.GetProductById(999);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateProduct_ValidDto_ReturnsCreatedAtAction()
    {
        var dto = new CreateProductDto { Name = "Teclado", Price = 250m, Stock = 5 };
        _serviceMock.Setup(s => s.CreateAsync(dto))
            .ReturnsAsync(new ProductDto { Id = 15, Name = "Teclado", Price = 250m, Stock = 5 });

        var result = await _controller.CreateProduct(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(nameof(ProductsController.GetProductById), createdResult.ActionName);
    }

    [Fact]
    public async Task CreateProduct_InvalidDto_ReturnsBadRequest()
    {
        var dto = new CreateProductDto { Name = string.Empty };
        _serviceMock.Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new ArgumentException("El nombre del producto es obligatorio."));

        var result = await _controller.CreateProduct(dto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateProduct_ExistingId_ReturnsOk()
    {
        var dto = new UpdateProductDto { Name = "Laptop Pro", Price = 1500m, Stock = 2 };
        _serviceMock.Setup(s => s.UpdateAsync(1, dto))
            .ReturnsAsync(new ProductDto { Id = 1, Name = "Laptop Pro", Price = 1500m, Stock = 2 });

        var result = await _controller.UpdateProduct(1, dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal("Laptop Pro", Assert.IsType<ProductDto>(okResult.Value).Name);
    }

    [Fact]
    public async Task UpdateProduct_NonExistingId_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.UpdateAsync(999, It.IsAny<UpdateProductDto>()))
            .ReturnsAsync((ProductDto?)null);

        var result = await _controller.UpdateProduct(999, new UpdateProductDto { Name = "X" });

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteProduct_ExistingId_ReturnsNoContent()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteProduct(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_NonExistingId_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

        var result = await _controller.DeleteProduct(999);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}