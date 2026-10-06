using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;

namespace TiendaOnline.Infrastructure.Repositories;

/// <summary>
/// Implementación que consulta la FakeDatabase para obtener el directorio de usuarios.
/// </summary>
public class DirectorioRepository : IDirectorioRepository
{
    public Task<IEnumerable<PerfilDirectorio>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        // Simulamos una latencia de red de 1 segundo para probar el estado de carga en el frontend
        Thread.Sleep(1000); 
        return Task.FromResult<IEnumerable<PerfilDirectorio>>(FakeDatabase.Directorios);
    }
}