using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;

namespace TiendaOnline.Infrastructure.Repositories;

public class ProductoGlobalRepository : IProductoGlobalRepository
{
    public Task<IEnumerable<ProductoGlobal>> ObtenerTodosAsync(CancellationToken ct = default) 
    {
        return Task.FromResult<IEnumerable<ProductoGlobal>>(FakeDatabase.ProductosGlobales);
    }
}