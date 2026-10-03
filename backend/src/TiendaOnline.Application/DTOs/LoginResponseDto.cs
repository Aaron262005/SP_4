namespace TiendaOnline.Application.DTOs;

/// <summary>
/// Respuesta de un login exitoso (200 OK): solo el token de acceso.
/// El frontend decodifica el token para obtener el ID del usuario.
/// </summary>
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
}