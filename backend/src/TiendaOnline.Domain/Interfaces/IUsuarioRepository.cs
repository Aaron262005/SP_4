using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Domain.Interfaces;

/// <summary>
/// Contrato de acceso a datos de usuarios.
/// Application usa esta interfaz; Infrastructure la implementa (Inversión de Dependencias).
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>Busca un usuario por su nombre de usuario. Devuelve null si no existe.</summary>
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);

    /// <summary>Busca un usuario por su identificador. Devuelve null si no existe.</summary>
    Task<Usuario?> ObtenerPorIdAsync(int id);
}