using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;

namespace TiendaOnline.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de usuarios usando la BD simulada.
/// Cumple el contrato IUsuarioRepository definido en Domain.
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly FakeDatabase _db;

    // La BD simulada se recibe por inyección de dependencias.
    public UsuarioRepository(FakeDatabase db)
    {
        _db = db;
    }

    public Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
    {
        // Búsqueda sin distinguir mayúsculas/minúsculas en el nombre de usuario.
        var usuario = _db.Usuarios.FirstOrDefault(u =>
            string.Equals(u.NombreUsuario, nombreUsuario, StringComparison.OrdinalIgnoreCase));

        // Task.FromResult: la BD es en memoria, así que no hay espera real.
        return Task.FromResult(usuario);
    }

    public Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        var usuario = _db.Usuarios.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(usuario);
    }
}