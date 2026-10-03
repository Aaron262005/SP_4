using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;

namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>
/// Caso de uso de LECTURA: obtener la información de un usuario por su ID.
/// No modifica nada (CQRS: las Queries solo leen).
/// </summary>
public class ObtenerUsuarioPorIdQueryHandler : IQueryHandler<ObtenerUsuarioPorIdQuery, UsuarioDto?>
{
    private readonly IUsuarioRepository _usuarios;

    public ObtenerUsuarioPorIdQueryHandler(IUsuarioRepository usuarios)
    {
        _usuarios = usuarios;
    }

    public async Task<UsuarioDto?> HandleAsync(
        ObtenerUsuarioPorIdQuery query, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(query.Id);
        if (usuario is null) return null;

        // Se convierte la entidad en DTO: la contraseña nunca sale de aquí.
        return new UsuarioDto
        {
            Id = usuario.Id,
            NombreUsuario = usuario.NombreUsuario,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo
        };
    }
}