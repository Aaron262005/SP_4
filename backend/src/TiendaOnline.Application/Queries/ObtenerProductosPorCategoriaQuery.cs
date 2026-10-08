namespace TiendaOnline.Application.Queries;

/// <summary>
/// Consulta de solo lectura: pide los productos de una categoría.
/// </summary>
/// <param name="Categoria">Nombre de la categoría a filtrar.</param>
public record ObtenerProductosPorCategoriaQuery(string Categoria);