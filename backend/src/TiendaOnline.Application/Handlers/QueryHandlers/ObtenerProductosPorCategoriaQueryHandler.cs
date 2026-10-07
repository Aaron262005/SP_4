using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;

namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>
/// Ejecuta ObtenerProductosPorCategoriaQuery: pide al repositorio los productos
/// de la categoría y los convierte a DTO. Si la categoría no existe devuelve una lista vacía.
/// </summary>
public class ObtenerProductosPorCategoriaQueryHandler
    : IQueryHandler<ObtenerProductosPorCategoriaQuery, IReadOnlyList<ProductoDto>>
{
    private readonly ICategoriaRepository _repositorio;

    public ObtenerProductosPorCategoriaQueryHandler(ICategoriaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IReadOnlyList<ProductoDto>> HandleAsync(
        ObtenerProductosPorCategoriaQuery query,
        CancellationToken cancellationToken = default)
    {
        var productos = await _repositorio.ObtenerProductosPorCategoriaAsync(
            query.Categoria, cancellationToken);

        // Se convierte cada entidad en DTO antes de salir de la capa Application.
        return productos
            .Select(p => new ProductoDto(p.Id, p.Titulo, p.Precio, p.Descripcion, p.Categoria, p.Imagen))
            .ToList();
    }
}