using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;

namespace TiendaOnline.Api.Controllers;

/// <summary>
/// Endpoints de autenticación. Es "delgado": no tiene lógica de negocio,
/// solo convierte la petición HTTP en un Command y traduce el resultado a HTTP.
/// </summary>
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    // Depende de la INTERFAZ del handler, no de la clase concreta (principio D).
    private readonly ICommandHandler<LoginCommand, LoginResponseDto?> _loginHandler;

    public AuthController(ICommandHandler<LoginCommand, LoginResponseDto?> loginHandler)
    {
        _loginHandler = loginHandler;
    }

    /// <summary>Inicia sesión y devuelve el token de acceso.</summary>
    /// <response code="200">Credenciales correctas: devuelve el token.</response>
    /// <response code="400">Faltan datos (usuario o contraseña vacíos).</response>
    /// <response code="401">Usuario o contraseña inválidos.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto solicitud, CancellationToken cancellationToken)
    {
        // Si el DTO no cumple las validaciones, [ApiController] ya respondió 400 antes de llegar aquí.
        var comando = new LoginCommand(solicitud.NombreUsuario, solicitud.Contrasena);
        var respuesta = await _loginHandler.HandleAsync(comando, cancellationToken);

        // null = credenciales inválidas => 401 Unauthorized.
        if (respuesta is null)
            return Unauthorized(new { mensaje = "Usuario o contraseña inválidos" });

        // Éxito => 200 OK con el token.
        return Ok(respuesta);
    }
}