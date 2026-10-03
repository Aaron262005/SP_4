namespace TiendaOnline.Application.Commands;

/// <summary>
/// COMMAND (CQRS): intención de iniciar sesión.
/// Solo transporta los datos necesarios; no ejecuta ninguna lógica.
/// </summary>
public record LoginCommand(string NombreUsuario, string Contrasena);
