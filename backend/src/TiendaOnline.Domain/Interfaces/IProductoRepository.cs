using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Domain.Interfaces;

/// <summary>
/// Contrato compartido del inventario, independiente de la persistencia.
/// Vive en Domain para que Application dependa de esta interfaz y nunca de la
/// implementación (que está en Infrastructure).
/// </summary>
public interface IProductoRepository
{
    /// <summary>Devuelve todos los productos disponibles (US03: catálogo).</summary>
    Task<IReadOnlyList<Producto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    /// <summary>Busca un producto por su identificador; devuelve null si no existe.</summary>
    Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Registra un producto nuevo, genera su ID y lo devuelve (US06: agregar).</summary>
    Task<Producto> AgregarAsync(string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default);

    /// <summary>Actualiza un producto existente; devuelve null si no existe (US07: editar).</summary>
    Task<Producto?> ActualizarAsync(int id, string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default);

    /// <summary>Elimina un producto y lo devuelve; devuelve null si no existe (US08: eliminar).</summary>
    Task<Producto?> EliminarAsync(int id, CancellationToken cancellationToken = default);
}