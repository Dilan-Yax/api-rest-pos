namespace ApiRestPos.Domain.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string Role { get; set; } = "User";
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}