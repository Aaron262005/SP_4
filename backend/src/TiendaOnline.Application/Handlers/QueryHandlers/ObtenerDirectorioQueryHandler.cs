using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;

namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>
/// Manejador que ejecuta la consulta del directorio y mapea la entidad al DTO.
/// </summary>
public class ObtenerDirectorioQueryHandler : IQueryHandler<ObtenerDirectorioQuery, IEnumerable<DirectorioUsuarioDto>>
{
    private readonly IDirectorioRepository _repository;

    public ObtenerDirectorioQueryHandler(IDirectorioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DirectorioUsuarioDto>> HandleAsync(ObtenerDirectorioQuery query, CancellationToken cancellationToken = default)
    {
        var perfiles = await _repository.ObtenerTodosAsync(cancellationToken);
        
        // Mapeo manual de Entidad a DTO para proteger la capa de Dominio
        return perfiles.Select(p => new DirectorioUsuarioDto(
            p.UsuarioId,
            p.NombreCompleto,
            p.Correo,
            p.Telefono,
            new DireccionDto(p.Ubicacion.Calle, p.Ubicacion.Ciudad, p.Ubicacion.Coordenadas)
        ));
    }
}