using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Domain.Interfaces;

/// <summary>
/// Contrato para acceder a los productos. Vive en Domain para que Application
/// dependa de esta interfaz y nunca de la implementación (que está en Infrastructure).
/// </summary>
public interface IProductoRepository
{
    /// <summary>Devuelve todos los productos disponibles.</summary>
    Task<IReadOnlyList<Producto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
}
