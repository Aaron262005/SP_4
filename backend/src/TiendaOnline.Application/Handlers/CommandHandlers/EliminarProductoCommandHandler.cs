using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.CommandHandlers;

/// <summary>Ejecuta EliminarProducto mediante el contrato del repositorio (CQRS y SOLID).</summary>
public class EliminarProductoCommandHandler(IProductoRepository productos) : ICommandHandler<EliminarProductoCommand, ProductoDto?>
{
    public async Task<ProductoDto?> HandleAsync(EliminarProductoCommand command, CancellationToken cancellationToken = default)
    {
        var producto = await productos.EliminarAsync(command.Id, cancellationToken);
        return producto is null ? null : ProductoDto.Desde(producto);
    }
}
