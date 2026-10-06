using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;

namespace TiendaOnline.Infrastructure.Repositories;

public class CarritoGlobalRepository : ICarritoGlobalRepository
{
    public Task<IEnumerable<CarritoGlobal>> ObtenerTodosAsync(CancellationToken ct = default) 
    {
        return Task.FromResult<IEnumerable<CarritoGlobal>>(FakeDatabase.CarritosGlobales);
    }
}