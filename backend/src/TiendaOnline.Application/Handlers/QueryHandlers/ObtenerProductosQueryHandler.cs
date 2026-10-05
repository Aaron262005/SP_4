using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;

namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>
/// Ejecuta la consulta ObtenerProductosQuery: pide los productos al repositorio
/// y los convierte a DTO. Depende solo de la interfaz del repositorio (principio D).
/// </summary>
public class ObtenerProductosQueryHandler
    : IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>>
{
    private readonly IProductoRepository _repositorio;

    public ObtenerProductosQueryHandler(IProductoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IReadOnlyList<ProductoDto>> HandleAsync(
        ObtenerProductosQuery query,
        CancellationToken cancellationToken = default)
    {
        var productos = await _repositorio.ObtenerTodosAsync(cancellationToken);

        // Se convierte cada entidad en un DTO antes de salir de la capa Application.
        return productos
            .Select(p => new ProductoDto(p.Id, p.Titulo, p.Precio, p.Descripcion, p.Categoria, p.Imagen))
            .ToList();
    }
}