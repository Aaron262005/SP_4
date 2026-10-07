using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;

namespace TiendaOnline.Api.Controllers;

/// <summary>
/// Endpoints de categorías de productos (US04).
/// Controller delgado: crea la Query, llama al handler y devuelve el código HTTP.
/// Es un controller aparte de ProductosController para no modificar el de la US03.
/// </summary>
[ApiController]
[Route("products")]
public class ProductosCategoriasController : ControllerBase
{
    private readonly IQueryHandler<ObtenerCategoriasQuery, IReadOnlyList<string>> _categoriasHandler;
    private readonly IQueryHandler<ObtenerProductosPorCategoriaQuery, IReadOnlyList<ProductoDto>> _porCategoriaHandler;

    public ProductosCategoriasController(
        IQueryHandler<ObtenerCategoriasQuery, IReadOnlyList<string>> categoriasHandler,
        IQueryHandler<ObtenerProductosPorCategoriaQuery, IReadOnlyList<ProductoDto>> porCategoriaHandler)
    {
        _categoriasHandler = categoriasHandler;
        _porCategoriaHandler = porCategoriaHandler;
    }

    /// <summary>Devuelve los nombres de las categorías disponibles.</summary>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(IReadOnlyList<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerCategorias(CancellationToken cancellationToken)
    {
        var categorias = await _categoriasHandler.HandleAsync(
            new ObtenerCategoriasQuery(), cancellationToken);

        return Ok(categorias);
    }

    /// <summary>Devuelve solo los productos de la categoría indicada (lista vacía si no existe).</summary>
    [HttpGet("category/{categoria}")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorCategoria(string categoria, CancellationToken cancellationToken)
    {
        var productos = await _porCategoriaHandler.HandleAsync(
            new ObtenerProductosPorCategoriaQuery(categoria), cancellationToken);

        return Ok(productos);
    }
}
