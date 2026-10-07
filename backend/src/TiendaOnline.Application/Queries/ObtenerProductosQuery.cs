namespace TiendaOnline.Application.Queries;

/// <summary>
/// Consulta de solo lectura: pide la lista completa de productos.
/// La usan el catálogo (US03) y las pantallas que muestran el detalle.
/// No lleva datos porque no se filtra nada.
/// </summary>
public record ObtenerProductosQuery();