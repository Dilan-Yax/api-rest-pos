using ApiRestPos.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiRestPos.Persistence.Context;

public class PosDbContext : DbContext
{
    public PosDbContext(DbContextOptions<PosDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Barcode).HasMaxLength(50);
            entity.Property(p => p.Price).HasPrecision(18, 2);
            entity.Property(p => p.Category).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).HasMaxLength(50);
        });

        SeedProducts(modelBuilder);
    }

    private static void SeedProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop Gamer", Barcode = "75010001", Price = 12500.00m, Stock = 15, Category = "Electrónica" },
            new Product { Id = 2, Name = "Mouse Inalámbrico", Barcode = "75010002", Price = 250.00m, Stock = 40, Category = "Accesorios" },
            new Product { Id = 3, Name = "Teclado Mecánico", Barcode = "75010003", Price = 650.00m, Stock = 25, Category = "Accesorios" },
            new Product { Id = 4, Name = "Monitor 27 Pulgadas", Barcode = "75010004", Price = 3200.00m, Stock = 10, Category = "Monitores" });
    }
}