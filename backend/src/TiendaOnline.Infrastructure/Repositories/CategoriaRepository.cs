using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;

namespace TiendaOnline.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de categorías sobre la base de datos simulada.
/// Las categorías no tienen su propia "tabla": se obtienen de los productos existentes.
/// </summary>
public class CategoriaRepository : ICategoriaRepository
{
    private readonly FakeDatabase _baseDeDatos;

    public CategoriaRepository(FakeDatabase baseDeDatos)
    {
        _baseDeDatos = baseDeDatos;
    }

    public Task<IReadOnlyList<string>> ObtenerCategoriasAsync(CancellationToken cancellationToken = default)
    {
        // Nombres únicos y ordenados alfabéticamente.
        IReadOnlyList<string> categorias = _baseDeDatos.Productos
            .Select(p => p.Categoria)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        return Task.FromResult(categorias);
    }

    public Task<IReadOnlyList<Producto>> ObtenerProductosPorCategoriaAsync(
        string categoria, CancellationToken cancellationToken = default)
    {
        // La comparación ignora mayúsculas y minúsculas.
        IReadOnlyList<Producto> productos = _baseDeDatos.Productos
            .Where(p => string.Equals(p.Categoria, categoria, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult(productos);
    }
}