using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;

namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>
/// Ejecuta ObtenerCategoriasQuery: pide las categorías al repositorio.
/// Depende solo de la interfaz del repositorio (principio D).
/// </summary>
public class ObtenerCategoriasQueryHandler
    : IQueryHandler<ObtenerCategoriasQuery, IReadOnlyList<string>>
{
    private readonly ICategoriaRepository _repositorio;

    public ObtenerCategoriasQueryHandler(ICategoriaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IReadOnlyList<string>> HandleAsync(
        ObtenerCategoriasQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _repositorio.ObtenerCategoriasAsync(cancellationToken);
    }
}