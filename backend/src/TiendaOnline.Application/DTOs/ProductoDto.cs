using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Application.DTOs;

/// <summary>
/// Datos de un producto que viajan por la API hacia el frontend.
/// Se usa un DTO para no exponer nunca la entidad de dominio:
/// las entidades nunca salen del servidor.
/// </summary>
/// <param name="Id">Identificador del producto.</param>
/// <param name="Titulo">Nombre del producto.</param>
/// <param name="Precio">Precio del producto.</param>
/// <param name="Descripcion">Descripción breve.</param>
/// <param name="Categoria">Categoría a la que pertenece.</param>
/// <param name="Imagen">URL de la imagen del producto.</param>
public record ProductoDto(
    int Id,
    string Titulo,
    decimal Precio,
    string Descripcion,
    string Categoria,
    string Imagen)
{
    /// <summary>
    /// Convierte una entidad Producto en su DTO. Es el único punto de conversión,
    /// así los handlers y controladores no repiten el mapeo campo por campo.
    /// </summary>
    public static ProductoDto Desde(Producto p) =>
        new(p.Id, p.Titulo, p.Precio, p.Descripcion, p.Categoria, p.Imagen);
}