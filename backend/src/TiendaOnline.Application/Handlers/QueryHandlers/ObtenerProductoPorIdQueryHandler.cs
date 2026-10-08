using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>Consulta los productos y transforma las entidades a DTO.</summary>
public class ObtenerProductoPorIdQueryHandler(IProductoRepository productos) : IQueryHandler<ObtenerProductoPorIdQuery, ProductoDto?>
{
    public async Task<ProductoDto?> HandleAsync(ObtenerProductoPorIdQuery query, CancellationToken cancellationToken = default)
    {
        var producto = await productos.ObtenerPorIdAsync(query.Id, cancellationToken);
        return producto is null ? null : ProductoDto.Desde(producto);
    }
}
