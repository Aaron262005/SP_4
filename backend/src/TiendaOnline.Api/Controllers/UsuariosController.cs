using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;

namespace TiendaOnline.Api.Controllers;

/// <summary>
/// Endpoints de consulta de usuarios.
/// Para el trabajo de clase no se valida el token aquí; en un sistema real se protegería.
/// </summary>
[ApiController]
[Route("usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IQueryHandler<ObtenerUsuarioPorIdQuery, UsuarioDto?> _obtenerPorIdHandler;

    public UsuariosController(IQueryHandler<ObtenerUsuarioPorIdQuery, UsuarioDto?> obtenerPorIdHandler)
    {
        _obtenerPorIdHandler = obtenerPorIdHandler;
    }

    /// <summary>Devuelve la información de un usuario por su ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        var usuario = await _obtenerPorIdHandler.HandleAsync(
            new ObtenerUsuarioPorIdQuery(id), cancellationToken);

        return usuario is null
            ? NotFound(new { mensaje = $"No existe el usuario con ID {id}." })
            : Ok(usuario);
    }
}