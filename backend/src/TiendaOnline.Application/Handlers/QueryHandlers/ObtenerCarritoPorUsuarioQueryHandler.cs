using TiendaOnline.Application.DTOs.Carrito;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries.Carrito;
using TiendaOnline.Domain.Interfaces.Carrito;

namespace TiendaOnline.Application.Handlers.QueryHandlers.Carrito;

public class ObtenerCarritoPorUsuarioQueryHandler : IQueryHandler<ObtenerCarritoPorUsuarioQuery, List<ItemCarritoDto>>
{
    private readonly ICarritoRepository _carritoRepository;

    public ObtenerCarritoPorUsuarioQueryHandler(ICarritoRepository carritoRepository)
    {
        _carritoRepository = carritoRepository;
    }

    public async Task<List<ItemCarritoDto>> HandleAsync(ObtenerCarritoPorUsuarioQuery query, CancellationToken cancellationToken = default)
    {
        var items = await _carritoRepository.ObtenerPorUsuarioIdAsync(query.UsuarioId, cancellationToken);

        return items.Select(item => new ItemCarritoDto
        {
            Id = item.Id,
            UsuarioId = item.UsuarioId,
            ProductoId = item.ProductoId,
            NombreProducto = item.NombreProducto,
            PrecioUnitario = item.PrecioUnitario,
            Cantidad = item.Cantidad
        }).ToList();
    }
}