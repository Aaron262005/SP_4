using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;

namespace TiendaOnline.Api.Controllers;

/// <summary>
/// Controlador delgado para exponer el endpoint del directorio.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DirectorioController : ControllerBase
{
    private readonly IQueryHandler<ObtenerDirectorioQuery, IEnumerable<DirectorioUsuarioDto>> _handler;

    public DirectorioController(IQueryHandler<ObtenerDirectorioQuery, IEnumerable<DirectorioUsuarioDto>> handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerDirectorio(CancellationToken cancellationToken)
    {
        var query = new ObtenerDirectorioQuery();
        var resultado = await _handler.HandleAsync(query, cancellationToken);
        return Ok(resultado);
    }
}