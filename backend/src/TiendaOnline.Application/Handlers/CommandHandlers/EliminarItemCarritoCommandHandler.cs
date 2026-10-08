using TiendaOnline.Application.Commands.Carrito;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces.Carrito;

namespace TiendaOnline.Application.Handlers.CommandHandlers.Carrito;

public class EliminarItemCarritoCommandHandler : ICommandHandler<EliminarItemCarritoCommand, bool>
{
    private readonly ICarritoRepository _carritoRepository;

    public EliminarItemCarritoCommandHandler(ICarritoRepository carritoRepository)
    {
        _carritoRepository = carritoRepository;
    }

    public async Task<bool> HandleAsync(EliminarItemCarritoCommand command, CancellationToken cancellationToken = default)
    {
        var item = await _carritoRepository.ObtenerPorIdAsync(command.ItemId, cancellationToken);
        if (item == null) return false;

        await _carritoRepository.EliminarAsync(command.ItemId, cancellationToken);
        return true;
    }
}