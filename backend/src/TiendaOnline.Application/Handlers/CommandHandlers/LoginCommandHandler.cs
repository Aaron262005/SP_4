using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces;

namespace TiendaOnline.Application.Handlers.CommandHandlers;

/// <summary>
/// Caso de uso: iniciar sesión (US01).
/// Una sola responsabilidad: validar credenciales y devolver un token.
/// Devuelve null cuando las credenciales son inválidas (el controller responde 401).
/// </summary>
public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponseDto?>
{
    // Dependencias como INTERFACES: el handler no sabe si los datos vienen de
    // una lista en memoria o de una BD real.
    private readonly IUsuarioRepository _usuarios;
    private readonly ITokenService _tokens;

    // Las dependencias llegan por el constructor (inyección de dependencias).
    public LoginCommandHandler(IUsuarioRepository usuarios, ITokenService tokens)
    {
        _usuarios = usuarios;
        _tokens = tokens;
    }

    public async Task<LoginResponseDto?> HandleAsync(
        LoginCommand command, CancellationToken cancellationToken = default)
    {
        // 1. Buscar al usuario en la "base de datos".
        var usuario = await _usuarios.ObtenerPorNombreUsuarioAsync(command.NombreUsuario);

        // 2. Si no existe o la contraseña no coincide: credenciales inválidas.
        //    Se responde igual en ambos casos para no revelar qué dato falló.
        if (usuario is null || !usuario.ValidarContrasena(command.Contrasena))
            return null;

        // 3. Credenciales correctas: generar el token y devolverlo.
        return new LoginResponseDto { Token = _tokens.GenerarToken(usuario) };
    }
}