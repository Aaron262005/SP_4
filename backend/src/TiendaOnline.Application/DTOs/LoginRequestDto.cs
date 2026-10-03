using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Application.DTOs;

/// <summary>
/// Datos que el frontend envía a POST /auth/login.
/// Las anotaciones validan la entrada automáticamente (responde 400 si faltan datos)
/// y Swagger las muestra como campos obligatorios.
/// </summary>
public class LoginRequestDto
{
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [StringLength(50, ErrorMessage = "El usuario no puede superar 50 caracteres.")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, ErrorMessage = "La contraseña no puede superar 100 caracteres.")]
    public string Contrasena { get; set; } = string.Empty;
}