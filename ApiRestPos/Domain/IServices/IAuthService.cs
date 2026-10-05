using ApiRestPos.Dtos;

namespace ApiRestPos.Domain.IServices;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterUsuarioDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}