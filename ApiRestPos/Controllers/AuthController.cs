using ApiRestPos.Domain.IServices;
using ApiRestPos.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestPos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterUsuarioDto dto)
    {
        try
        {
            var result = await _authService.RegisterAsync(dto);
            return Created("", result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result is null)
        {
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        return Ok(result);
    }
}