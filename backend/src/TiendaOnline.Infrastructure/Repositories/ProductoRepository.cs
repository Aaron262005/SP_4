using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;

namespace TiendaOnline.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de productos sobre la base de datos simulada.
/// Debe registrarse como Singleton: el candado y el último ID viven en esta instancia,
/// y así las escrituras y la asignación de IDs son atómicas.
/// Puede sustituir a IProductoRepository sin que nadie más note la diferencia (principio L).
/// </summary>
public class ProductoRepository(FakeDatabase db) : IProductoRepository
{
    // Candado que protege la lista compartida cuando varias peticiones llegan a la vez.
    private readonly object _candado = new();

    // Último ID asignado; se calcula una vez a partir de los productos de prueba.
    private int _ultimoId = db.Productos.Select(p => p.Id).DefaultIfEmpty(0).Max();

    /// <summary>US03: devuelve una copia de la lista para que nadie la modifique por fuera.</summary>
    public Task<IReadOnlyList<Producto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado) return Task.FromResult<IReadOnlyList<Producto>>(db.Productos.ToArray());
    }

    /// <summary>Busca un producto por ID; devuelve null si no existe.</summary>
    public Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado) return Task.FromResult(db.Productos.FirstOrDefault(p => p.Id == id));
    }

    /// <summary>US06: crea el producto con el siguiente ID disponible y lo guarda.</summary>
    public Task<Producto> AgregarAsync(string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado)
        {
            // El constructor de Producto valida los datos; si son inválidos lanza excepción.
            var producto = new Producto(checked(_ultimoId + 1), titulo, precio, descripcion, categoria, imagen);
            db.Productos.Add(producto);
            _ultimoId = producto.Id;
            return Task.FromResult(producto);
        }
    }

    /// <summary>US07: reemplaza el producto por una instancia nueva; devuelve null si no existe.</summary>
    public Task<Producto?> ActualizarAsync(int id, string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado)
        {
            var indice = db.Productos.FindIndex(p => p.Id == id);
            if (indice < 0) return Task.FromResult<Producto?>(null);

            // Se crea otro Producto (y se valida) en lugar de mutar el anterior.
            var producto = new Producto(id, titulo, precio, descripcion, categoria, imagen);
            db.Productos[indice] = producto;
            return Task.FromResult<Producto?>(producto);
        }
    }

    /// <summary>US08: quita el producto de la lista y lo devuelve; null si no existía.</summary>
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