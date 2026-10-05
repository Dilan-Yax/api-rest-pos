using ApiRestPos.Domain.Models;

namespace ApiRestPos.Domain.IRepositories;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<Usuario> AddAsync(Usuario usuario);
}