using Microsoft.AspNetCore.Mvc;
using ApiRestPos.Models;

namespace ApiRestPos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> _products =
    [
        new Product { Id = 1, Name = "Laptop Gamer", Barcode = "75010001", Price = 12500.00m, Stock = 15, Category = "Electrónica" },
        new Product { Id = 2, Name = "Mouse Inalámbrico", Barcode = "75010002", Price = 250.00m, Stock = 40, Category = "Accesorios" },
        new Product { Id = 3, Name = "Teclado Mecánico", Barcode = "75010003", Price = 650.00m, Stock = 25, Category = "Accesorios" },
        new Product { Id = 4, Name = "Monitor 27 Pulgadas", Barcode = "75010004", Price = 3200.00m, Stock = 10, Category = "Monitores" }
    ];

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        return Ok(_products);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Product> GetProductById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound(new { message = $"Producto con id {id} no encontrado." });
        }
        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> CreateProduct([FromBody] Product newProduct)
    {
        newProduct.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
        _products.Add(newProduct);
        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
    }
}
