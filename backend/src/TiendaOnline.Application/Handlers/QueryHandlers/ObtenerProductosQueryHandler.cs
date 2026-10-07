using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;

namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>
/// Ejecuta la consulta ObtenerProductosQuery: pide los productos al repositorio
/// y los convierte a DTO. Depende solo de la interfaz del repositorio (principio D).
/// </summary>
public class ObtenerProductosQueryHandler(IProductoRepository productos)
    : IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>>
{
    public async Task<IReadOnlyList<ProductoDto>> HandleAsync(
        ObtenerProductosQuery query,
        CancellationToken cancellationToken = default)
    {
        // Se obtienen las entidades desde el repositorio.
        var entidades = await productos.ObtenerTodosAsync(cancellationToken);

        // Cada entidad se convierte en DTO con el mapeo centralizado antes de salir de Application.
        return entidades.Select(ProductoDto.Desde).ToArray();
    }
}