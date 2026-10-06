using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Domain.Interfaces;

public interface IProductoGlobalRepository 
{ 
    Task<IEnumerable<ProductoGlobal>> ObtenerTodosAsync(CancellationToken ct = default); 
}