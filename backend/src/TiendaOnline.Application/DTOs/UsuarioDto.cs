namespace TiendaOnline.Application.DTOs;

/// <summary>
/// Información pública de un usuario. Se usa un DTO (y no la entidad)
/// para NUNCA exponer la contraseña hacia afuera.
/// </summary>
public class UsuarioDto
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
}