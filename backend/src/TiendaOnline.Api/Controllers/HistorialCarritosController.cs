using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;

namespace TiendaOnline.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistorialCarritosController : ControllerBase
{
    private readonly IQueryHandler<ObtenerHistorialCarritosQuery, IEnumerable<HistorialCarritoDto>> _handler;
    
    public HistorialCarritosController(IQueryHandler<ObtenerHistorialCarritosQuery, IEnumerable<HistorialCarritoDto>> handler) 
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerHistorial(CancellationToken cancellationToken)
    {
        var query = new ObtenerHistorialCarritosQuery();
        var resultado = await _handler.HandleAsync(query, cancellationToken);
        return Ok(resultado);
    }
}