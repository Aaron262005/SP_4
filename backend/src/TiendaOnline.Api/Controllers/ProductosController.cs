using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Api.Filters;
using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
namespace TiendaOnline.Api.Controllers;

/// <summary>Adapta HTTP a CQRS. La autorización y persistencia viven fuera del controlador.</summary>
[ApiController]
[Route("products")]
public class ProductosController(
    IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>> listar,
    IQueryHandler<ObtenerProductoPorIdQuery, ProductoDto?> detalle,
    ICommandHandler<AgregarProductoCommand, ProductoDto> agregar) : ControllerBase


{
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken ct) => Ok(await listar.HandleAsync(new ObtenerProductosQuery(), ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obtener(int id, CancellationToken ct)
    {
        var producto = await detalle.HandleAsync(new ObtenerProductoPorIdQuery(id), ct);
        return producto is null ? NotFound(new { mensaje = "Producto no encontrado." }) : Ok(producto);
    }

    /// <summary>US06: crea un producto y devuelve su ID generado.</summary>
    [HttpPost]
    [ServiceFilter(typeof(AdministradorProductosFilter))]
    public async Task<IActionResult> Agregar(GuardarProductoDto datos, CancellationToken ct)
    {
        var producto = await agregar.HandleAsync(new AgregarProductoCommand(datos.Titulo, datos.Precio!.Value, datos.Descripcion, datos.Categoria, datos.Imagen), ct);
        return CreatedAtAction(nameof(Obtener), new { id = producto.Id }, producto);
    }

}
