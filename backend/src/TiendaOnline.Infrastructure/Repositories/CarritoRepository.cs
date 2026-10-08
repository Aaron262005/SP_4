using TiendaOnline.Domain.Entities.Carrito;
using TiendaOnline.Domain.Interfaces.Carrito;
using TiendaOnline.Infrastructure.Persistence;

namespace TiendaOnline.Infrastructure.Repositories.Carrito;

/// <summary>
/// Repositorio de Carrito respaldado por FakeDatabase (Lista en memoria).
/// Implements ICarritoRepository (SOLID L y D).
/// </summary>
public class CarritoRepository : ICarritoRepository
{
    public Task<List<ItemCarrito>> ObtenerPorUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        var items = FakeDatabase.ItemsCarrito.Where(i => i.UsuarioId == usuarioId).ToList();
        return Task.FromResult(items);
    }

    public Task<ItemCarrito?> ObtenerPorUsuarioYProductoAsync(int usuarioId, int productoId, CancellationToken cancellationToken = default)
    {
        var item = FakeDatabase.ItemsCarrito.FirstOrDefault(i => i.UsuarioId == usuarioId && i.ProductoId == productoId);
        return Task.FromResult(item);
    }

    public Task<ItemCarrito> AgregarAsync(ItemCarrito item, CancellationToken cancellationToken = default)
    {
        // Generar un ID autonumérico
        int nuevoId = FakeDatabase.ItemsCarrito.Any() ? FakeDatabase.ItemsCarrito.Max(i => i.Id) + 1 : 1;
        
        var nuevoItem = new ItemCarrito(nuevoId, item.UsuarioId, item.ProductoId, item.NombreProducto, item.PrecioUnitario, item.Cantidad);
        FakeDatabase.ItemsCarrito.Add(nuevoItem);

        return Task.FromResult(nuevoItem);
    }

    public Task ActualizarAsync(ItemCarrito item, CancellationToken cancellationToken = default)
    {
        // En FakeDatabase (lista en memoria C#) las referencias se actualizan automáticamente
        return Task.CompletedTask;
    }

    public Task<ItemCarrito?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = FakeDatabase.ItemsCarrito.FirstOrDefault(i => i.Id == id);
        return Task.FromResult(item);
    }

    public Task EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = FakeDatabase.ItemsCarrito.FirstOrDefault(i => i.Id == id);
        if (item != null)
        {
            FakeDatabase.ItemsCarrito.Remove(item);
        }
        return Task.CompletedTask;
    }
}