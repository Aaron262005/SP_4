using TiendaOnline.Application.Commands.Carrito;
using TiendaOnline.Application.DTOs.Carrito;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Entities.Carrito;
using TiendaOnline.Domain.Interfaces.Carrito;

namespace TiendaOnline.Application.Handlers.CommandHandlers.Carrito;

/// <summary>
/// Handler del caso de uso Agregar Ítem al Carrito.
/// Cumple con SOLID (S): Única responsabilidad de procesar este Command.
/// </summary>
public class AgregarItemCarritoCommandHandler : ICommandHandler<AgregarItemCarritoCommand, ItemCarritoDto?>
{
    private readonly ICarritoRepository _carritoRepository;

    // Inyección de dependencias por constructor (SOLID D)
    public AgregarItemCarritoCommandHandler(ICarritoRepository carritoRepository)
    {
        _carritoRepository = carritoRepository;
    }

    public async Task<ItemCarritoDto?> HandleAsync(AgregarItemCarritoCommand command, CancellationToken cancellationToken = default)
    {
        // 1. US09 Escenario 2: Validar si el producto ya existe en el carrito del cliente
        var itemExistente = await _carritoRepository.ObtenerPorUsuarioYProductoAsync(command.UsuarioId, command.ProductoId, cancellationToken);

        if (itemExistente != null)
        {
            // Sumar cantidad al ítem existente
            itemExistente.SumarCantidad(command.Cantidad);
            await _carritoRepository.ActualizarAsync(itemExistente, cancellationToken);

            return MapearADto(itemExistente);
        }

        // 2. US09 Escenario 1: Crear nuevo ítem si no existía en el carrito
        var nuevoItem = new ItemCarrito(0, command.UsuarioId, command.ProductoId, command.NombreProducto, command.PrecioUnitario, command.Cantidad);
        var itemGuardado = await _carritoRepository.AgregarAsync(nuevoItem, cancellationToken);

        return MapearADto(itemGuardado);
    }

    private static ItemCarritoDto MapearADto(ItemCarrito item) => new()
    {
        Id = item.Id,
        UsuarioId = item.UsuarioId,
        ProductoId = item.ProductoId,
        NombreProducto = item.NombreProducto,
        PrecioUnitario = item.PrecioUnitario,
        Cantidad = item.Cantidad
    };
}