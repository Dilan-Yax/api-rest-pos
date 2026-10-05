using Microsoft.AspNetCore.Mvc;

namespace ApiRestPos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PosStatusController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            service = "API REST POS (Punto de Venta)",
            status = "Online",
            framework = ".NET 9 / 10",
            timestamp = DateTime.UtcNow,
            openApi = "/openapi/v1.json",
            documentation = "/scalar/v1"
        });
    }
}

