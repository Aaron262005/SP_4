using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;

namespace TiendaOnline.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de productos sobre la base de datos simulada.
/// Puede sustituir a IProductoRepository sin que nadie más note la diferencia (principio L).
/// </summary>
public class ProductoRepository : IProductoRepository
{
    private readonly FakeDatabase _baseDeDatos;

    public ProductoRepository(FakeDatabase baseDeDatos)
    {
        _baseDeDatos = baseDeDatos;
    }

    public Task<IReadOnlyList<Producto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        // Se devuelve una copia para que nadie modifique la lista original por fuera.
        IReadOnlyList<Producto> productos = _baseDeDatos.Productos.ToList();
        return Task.FromResult(productos);
    }
}