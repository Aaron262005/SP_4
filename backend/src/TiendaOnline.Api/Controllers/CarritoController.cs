using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Application.Commands.Carrito;
using TiendaOnline.Application.DTOs.Carrito;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries.Carrito;

namespace TiendaOnline.Api.Controllers;

[ApiController]
[Route("api/carrito")]
public class CarritoController : ControllerBase
{
    private readonly ICommandHandler<AgregarItemCarritoCommand, ItemCarritoDto?> _agregarHandler;
    private readonly ICommandHandler<ActualizarCantidadItemCommand, ItemCarritoDto?> _actualizarHandler;
    private readonly ICommandHandler<EliminarItemCarritoCommand, bool> _eliminarHandler;
    private readonly IQueryHandler<ObtenerCarritoPorUsuarioQuery, List<ItemCarritoDto>> _obtenerCarritoHandler;

    public CarritoController(
        ICommandHandler<AgregarItemCarritoCommand, ItemCarritoDto?> agregarHandler,
        ICommandHandler<ActualizarCantidadItemCommand, ItemCarritoDto?> actualizarHandler,
        ICommandHandler<EliminarItemCarritoCommand, bool> eliminarHandler,
        IQueryHandler<ObtenerCarritoPorUsuarioQuery, List<ItemCarritoDto>> obtenerCarritoHandler)
    {
        _agregarHandler = agregarHandler;
        _actualizarHandler = actualizarHandler;
        _eliminarHandler = eliminarHandler;
        _obtenerCarritoHandler = obtenerCarritoHandler;
    }

    /// <summary>
    /// GET /api/carrito/usuario/{usuarioId} - US10: Obtener carrito de un cliente
    /// </summary>
    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<ActionResult<List<ItemCarritoDto>>> ObtenerPorUsuario(int usuarioId)
    {
        var resultado = await _obtenerCarritoHandler.HandleAsync(new ObtenerCarritoPorUsuarioQuery(usuarioId));
        return Ok(resultado);
    }

    /// <summary>
    /// POST /api/carrito - US09: Agregar ítem al carrito
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ItemCarritoDto>> AgregarItem([FromBody] AgregarItemCarritoRequestDto dto)
    {
        var command = new AgregarItemCarritoCommand(dto.UsuarioId, dto.ProductoId, dto.NombreProducto, dto.PrecioUnitario, dto.Cantidad);
        var resultado = await _agregarHandler.HandleAsync(command);

        if (resultado == null)
            return BadRequest(new { mensaje = "No se pudo añadir el producto al carrito." });

        return CreatedAtAction(nameof(AgregarItem), new { id = resultado.Id }, resultado);
    }

    /// <summary>
    /// PUT /api/carrito/{id}/cantidad - US10: Modificar cantidad de un artículo
    /// </summary>
    [HttpPut("{id:int}/cantidad")]
    public async Task<ActionResult<ItemCarritoDto>> ActualizarCantidad(int id, [FromBody] int nuevaCantidad)
    {
        if (nuevaCantidad <= 0)
            return BadRequest(new { mensaje = "La cantidad debe ser mayor a cero." });

        var resultado = await _actualizarHandler.HandleAsync(new ActualizarCantidadItemCommand(id, nuevaCantidad));
        if (resultado == null)
            return NotFound(new { mensaje = "Ítem de carrito no encontrado." });

        return Ok(resultado);
    }

    /// <summary>
    /// DELETE /api/carrito/{id} - US10: Eliminar un artículo del carrito
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarItem(int id)
    {
        var exito = await _eliminarHandler.HandleAsync(new EliminarItemCarritoCommand(id));
        if (!exito)
            return NotFound(new { mensaje = "Ítem de carrito no encontrado." });

        return NoContent();
    }
}