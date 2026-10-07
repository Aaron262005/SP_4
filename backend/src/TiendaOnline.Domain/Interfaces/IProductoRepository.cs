using TiendaOnline.Domain.Entities;
namespace TiendaOnline.Domain.Interfaces;

/// <summary>Contrato compartido del inventario, independiente de la persistencia.</summary>
public interface IProductoRepository
{
    Task<IReadOnlyList<Producto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Producto> AgregarAsync(string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default);
    Task<Producto?> ActualizarAsync(int id, string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default);
    Task<Producto?> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
