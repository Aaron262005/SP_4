using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.CommandHandlers;

/// <summary>Ejecuta EditarProducto mediante el contrato del repositorio (CQRS y SOLID).</summary>
public class EditarProductoCommandHandler(IProductoRepository productos) : ICommandHandler<EditarProductoCommand, ProductoDto?>
{
    public async Task<ProductoDto?> HandleAsync(EditarProductoCommand command, CancellationToken cancellationToken = default)
    {
        var producto = await productos.ActualizarAsync(command.Id, command.Titulo, command.Precio, command.Descripcion, command.Categoria, command.Imagen, cancellationToken);
        return producto is null ? null : ProductoDto.Desde(producto);
    }
}
