namespace TiendaOnline.Application.Queries;

/// <summary>
/// QUERY (CQRS): consulta de solo lectura para obtener la información de un usuario.
/// </summary>
public record ObtenerUsuarioPorIdQuery(int Id);