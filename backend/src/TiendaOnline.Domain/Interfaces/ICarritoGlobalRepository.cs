using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Domain.Interfaces;

public interface ICarritoGlobalRepository 
{ 
    Task<IEnumerable<CarritoGlobal>> ObtenerTodosAsync(CancellationToken ct = default); 
}