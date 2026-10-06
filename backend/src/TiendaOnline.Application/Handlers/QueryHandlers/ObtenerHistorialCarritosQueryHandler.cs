using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;

namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>Ejecuta el cruce de datos exigido por la HU para obtener el título real del producto.</summary>
public class ObtenerHistorialCarritosQueryHandler : IQueryHandler<ObtenerHistorialCarritosQuery, IEnumerable<HistorialCarritoDto>>
{
    private readonly ICarritoGlobalRepository _carritoRepo;
    private readonly IProductoGlobalRepository _productoRepo;

    public ObtenerHistorialCarritosQueryHandler(ICarritoGlobalRepository carritoRepo, IProductoGlobalRepository productoRepo)
    {
        _carritoRepo = carritoRepo; 
        _productoRepo = productoRepo;
    }

    public async Task<IEnumerable<HistorialCarritoDto>> HandleAsync(ObtenerHistorialCarritosQuery query, CancellationToken cancellationToken = default)
    {
        var carritos = await _carritoRepo.ObtenerTodosAsync(cancellationToken);
        var productos = await _productoRepo.ObtenerTodosAsync(cancellationToken);

        // Cruce de información: ID del producto del carrito contra el catálogo de productos
        return carritos.Select(c => new HistorialCarritoDto(
            c.Id, 
            c.UsuarioId, 
            c.FechaCreacion,
            c.Items.Select(i => new ItemHistorialDto(
                i.ProductoId,
                productos.FirstOrDefault(p => p.Id == i.ProductoId)?.Titulo ?? "Producto Eliminado/Desconocido",
                i.Cantidad
            )).ToList()
        ));
    }
}