namespace ApiRestPos.Domain.Models;

public class SaleItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal => Quantity * UnitPrice;
}

public class Sale
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string CustomerName { get; set; } = "Cliente Final";
    public List<SaleItem> Items { get; set; } = [];
    public decimal Total => Items.Sum(i => i.Subtotal);
    public string PaymentMethod { get; set; } = "Efectivo";
}