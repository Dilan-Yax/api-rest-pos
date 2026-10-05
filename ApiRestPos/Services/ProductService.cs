using ApiRestPos.Domain.IRepositories;
using ApiRestPos.Domain.IServices;
using ApiRestPos.Domain.Models;
using ApiRestPos.Dtos;

namespace ApiRestPos.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? null : MapToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        Validate(dto.Name, dto.Price, dto.Stock);

        var product = new Product
        {
            Name = dto.Name,
            Barcode = dto.Barcode,
            Price = dto.Price,
            Stock = dto.Stock,
            Category = dto.Category,
            IsActive = dto.IsActive
        };

        var created = await _repository.AddAsync(product);
        return MapToDto(created);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        Validate(dto.Name, dto.Price, dto.Stock);

        var product = new Product
        {
            Id = id,
            Name = dto.Name,
            Barcode = dto.Barcode,
            Price = dto.Price,
            Stock = dto.Stock,
            Category = dto.Category,
            IsActive = dto.IsActive
        };

        var updated = await _repository.UpdateAsync(product);
        return updated is null ? null : MapToDto(updated);
    }

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);

    private static void Validate(string name, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentException("El precio no puede ser negativo.", nameof(price));
        }

        if (stock < 0)
        {
            throw new ArgumentException("El stock no puede ser negativo.", nameof(stock));
        }
    }

    private static ProductDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Barcode = product.Barcode,
        Price = product.Price,
        Stock = product.Stock,
        Category = product.Category,
        IsActive = product.IsActive
    };
}