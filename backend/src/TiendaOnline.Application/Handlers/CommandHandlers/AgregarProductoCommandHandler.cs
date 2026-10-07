using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.CommandHandlers;

/// <summary>Ejecuta AgregarProducto mediante el contrato del repositorio (CQRS y SOLID).</summary>
public class AgregarProductoCommandHandler(IProductoRepository productos) : ICommandHandler<AgregarProductoCommand, ProductoDto>
{
    public async Task<ProductoDto> HandleAsync(AgregarProductoCommand command, CancellationToken cancellationToken = default)
    {
        var producto = await productos.AgregarAsync(command.Titulo, command.Precio, command.Descripcion, command.Categoria, command.Imagen, cancellationToken);
        return ProductoDto.Desde(producto);
    }
}
