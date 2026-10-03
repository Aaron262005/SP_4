using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Application.Interfaces;

/// <summary>
/// Abstracción para generar tokens de acceso.
/// El handler depende de esta interfaz, no de cómo se genera el token (principio D).
/// </summary>
public interface ITokenService
{
    string GenerarToken(Usuario usuario);
}