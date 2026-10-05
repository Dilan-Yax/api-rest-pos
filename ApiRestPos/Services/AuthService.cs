using ApiRestPos.Domain.IRepositories;
using ApiRestPos.Domain.IServices;
using ApiRestPos.Domain.Models;
using ApiRestPos.Dtos;
using ApiRestPos.Utils;

namespace ApiRestPos.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repository;
    private readonly JwtTokenService _jwtTokenService;

    public AuthService(IUsuarioRepository repository, JwtTokenService jwtTokenService)
    {
        _repository = repository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUsuarioDto dto)
    {
        ValidateRegistration(dto);

        if (await _repository.EmailExistsAsync(dto.Email))
        {
            throw new ArgumentException("El email ya está registrado.", nameof(dto.Email));
        }

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email.ToLowerInvariant(),
            PasswordHash = PasswordHasher.Hash(dto.Password),
            FechaNacimiento = dto.FechaNacimiento
        };

        var created = await _repository.AddAsync(usuario);
        var token = _jwtTokenService.GenerateToken(created);

        return new AuthResponseDto
        {
            Token = token,
            Usuario = MapToDto(created)
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _repository.GetByEmailAsync(dto.Email);

        if (usuario is null || !PasswordHasher.Verify(dto.Password, usuario.PasswordHash))
        {
            return null;
        }

        var token = _jwtTokenService.GenerateToken(usuario);

        return new AuthResponseDto
        {
            Token = token,
            Usuario = MapToDto(usuario)
        };
    }

    private static void ValidateRegistration(RegisterUsuarioDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException("El nombre es obligatorio.", nameof(dto.Nombre));
        }

        if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
        {
            throw new ArgumentException("El email no es válido.", nameof(dto.Email));
        }

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8)
        {
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.", nameof(dto.Password));
        }

        var today = DateTime.UtcNow;
        var age = today.Year - dto.FechaNacimiento.Year;
        if (dto.FechaNacimiento.Date > today.AddYears(-age))
        {
            age--;
        }

        if (age < 18)
        {
            throw new ArgumentException("El usuario debe ser mayor de edad.", nameof(dto.FechaNacimiento));
        }
    }

    private static UsuarioDto MapToDto(Usuario usuario) => new()
    {
        Id = usuario.Id,
        Nombre = usuario.Nombre,
        Email = usuario.Email,
        FechaNacimiento = usuario.FechaNacimiento,
        Role = usuario.Role
    };
}