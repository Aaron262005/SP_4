using TiendaOnline.Domain.Entities.Carrito;

namespace TiendaOnline.Domain.Interfaces.Carrito;

public interface ICarritoRepository
{
    Task<List<ItemCarrito>> ObtenerPorUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken = default);
    Task<ItemCarrito?> ObtenerPorUsuarioYProductoAsync(int usuarioId, int productoId, CancellationToken cancellationToken = default);
    Task<ItemCarrito?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ItemCarrito> AgregarAsync(ItemCarrito item, CancellationToken cancellationToken = default);
    Task ActualizarAsync(ItemCarrito item, CancellationToken cancellationToken = default);
    Task EliminarAsync(int id, CancellationToken cancellationToken = default);
}