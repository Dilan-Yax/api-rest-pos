using ApiRestPos.Domain.IRepositories;
using ApiRestPos.Domain.Models;
using ApiRestPos.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiRestPos.Persistence.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly PosDbContext _context;

    public UsuarioRepository(PosDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
        => await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());

    public async Task<bool> EmailExistsAsync(string email)
        => await _context.Usuarios
            .AnyAsync(u => u.Email == email.ToLowerInvariant());

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }
}