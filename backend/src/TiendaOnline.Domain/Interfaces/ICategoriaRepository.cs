using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Domain.Interfaces;

/// <summary>
/// Contrato para consultar categorías de productos (US04).
/// Es una interfaz aparte de IProductoRepository para no modificar la de la US03
/// y para que cada contrato sea pequeño y específico.
/// </summary>
public interface ICategoriaRepository
{
    /// <summary>Devuelve los nombres de las categorías existentes, sin repetir.</summary>
    Task<IReadOnlyList<string>> ObtenerCategoriasAsync(CancellationToken cancellationToken = default);

    /// <summary>Devuelve solo los productos de la categoría indicada.</summary>
    Task<IReadOnlyList<Producto>> ObtenerProductosPorCategoriaAsync(
        string categoria, CancellationToken cancellationToken = default);
}
