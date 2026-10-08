using TiendaOnline.Application.Commands.Carrito;
using TiendaOnline.Application.DTOs.Carrito;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces.Carrito;

namespace TiendaOnline.Application.Handlers.CommandHandlers.Carrito;

public class ActualizarCantidadItemCommandHandler : ICommandHandler<ActualizarCantidadItemCommand, ItemCarritoDto?>
{
    private readonly ICarritoRepository _carritoRepository;

    public ActualizarCantidadItemCommandHandler(ICarritoRepository carritoRepository)
    {
        _carritoRepository = carritoRepository;
    }

    public async Task<ItemCarritoDto?> HandleAsync(ActualizarCantidadItemCommand command, CancellationToken cancellationToken = default)
    {
        var item = await _carritoRepository.ObtenerPorIdAsync(command.ItemId, cancellationToken);
        if (item == null) return null;

        item.ActualizarCantidad(command.NuevaCantidad);
        await _carritoRepository.ActualizarAsync(item, cancellationToken);

        return new ItemCarritoDto
        {
            Id = item.Id,
            UsuarioId = item.UsuarioId,
            ProductoId = item.ProductoId,
            NombreProducto = item.NombreProducto,
            PrecioUnitario = item.PrecioUnitario,
            Cantidad = item.Cantidad
        };
    }
}