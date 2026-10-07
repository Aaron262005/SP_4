using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;
namespace TiendaOnline.Infrastructure.Repositories;

/// <summary>Inventario en memoria. Singleton y bloqueo hacen atómicas las escrituras y la asignación de IDs.</summary>
public class ProductoRepository(FakeDatabase db) : IProductoRepository
{
    private readonly object _candado = new();
    private int _ultimoId = db.Productos.Select(p => p.Id).DefaultIfEmpty(0).Max();

    public Task<IReadOnlyList<Producto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado) return Task.FromResult<IReadOnlyList<Producto>>(db.Productos.ToArray());
    }
    public Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado) return Task.FromResult(db.Productos.FirstOrDefault(p => p.Id == id));
    }
    public Task<Producto> AgregarAsync(string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado)
        {
            var producto = new Producto(checked(_ultimoId + 1), titulo, precio, descripcion, categoria, imagen);
            db.Productos.Add(producto);
            _ultimoId = producto.Id;
            return Task.FromResult(producto);
        }
    }
    public Task<Producto?> ActualizarAsync(int id, string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado)
        {
            var indice = db.Productos.FindIndex(p => p.Id == id);
            if (indice < 0) return Task.FromResult<Producto?>(null);
            var producto = new Producto(id, titulo, precio, descripcion, categoria, imagen);
            db.Productos[indice] = producto;
            return Task.FromResult<Producto?>(producto);
        }
    }
    public Task<Producto?> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado)
        {
            var producto = db.Productos.FirstOrDefault(p => p.Id == id);
            if (producto is not null) db.Productos.Remove(producto);
            return Task.FromResult(producto);
        }
    }
}
