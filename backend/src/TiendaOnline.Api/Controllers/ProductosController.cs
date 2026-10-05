using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;

namespace TiendaOnline.Api.Controllers;

/// <summary>
/// Endpoints de consulta de productos.
/// Controller delgado: crea la Query, llama al handler y devuelve el código HTTP.
/// Para el trabajo de clase no se valida el token aquí, igual que en UsuariosController.
/// </summary>
[ApiController]
[Route("products")]
public class ProductosController : ControllerBase
{
    private readonly IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>> _obtenerProductosHandler;

    public ProductosController(
        IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>> obtenerProductosHandler)
    {
        _obtenerProductosHandler = obtenerProductosHandler;
    }

    /// <summary>Devuelve la lista completa de productos disponibles.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var productos = await _obtenerProductosHandler.HandleAsync(
            new ObtenerProductosQuery(), cancellationToken);

        return Ok(productos);
    }
}