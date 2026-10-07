using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>Consulta los productos y transforma las entidades a DTO.</summary>
public class ObtenerProductosQueryHandler(IProductoRepository productos) : IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>>
{
    public async Task<IReadOnlyList<ProductoDto>> HandleAsync(ObtenerProductosQuery query, CancellationToken cancellationToken = default)
    {
        return (await productos.ObtenerTodosAsync(cancellationToken)).Select(ProductoDto.Desde).ToArray();
    }
}
