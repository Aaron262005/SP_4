using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Domain.Interfaces;

/// <summary>
/// Contrato para obtener los perfiles extendidos del directorio.
/// </summary>
public interface IDirectorioRepository
{
    Task<IEnumerable<PerfilDirectorio>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
}