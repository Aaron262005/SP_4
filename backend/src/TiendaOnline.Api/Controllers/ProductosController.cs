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
    ICommandHandler<AgregarProductoCommand, ProductoDto> agregar,
    ICommandHandler<EditarProductoCommand, ProductoDto?> editar,
    ICommandHandler<EliminarProductoCommand, ProductoDto?> eliminar) : ControllerBase
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

    /// <summary>US07: actualiza un producto existente.</summary>
    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(AdministradorProductosFilter))]
    public async Task<IActionResult> Editar(int id, GuardarProductoDto datos, CancellationToken ct)
    {
        var producto = await editar.HandleAsync(new EditarProductoCommand(id, datos.Titulo, datos.Precio!.Value, datos.Descripcion, datos.Categoria, datos.Imagen), ct);
        return producto is null ? NotFound(new { mensaje = "Producto no encontrado." }) : Ok(producto);
    }

    /// <summary>US08: devuelve el objeto eliminado para confirmar el resultado.</summary>
    [HttpDelete("{id:int}")]
    [ServiceFilter(typeof(AdministradorProductosFilter))]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        var producto = await eliminar.HandleAsync(new EliminarProductoCommand(id), ct);
        return producto is null ? NotFound(new { mensaje = "Producto no encontrado." }) : Ok(producto);
    }
}
