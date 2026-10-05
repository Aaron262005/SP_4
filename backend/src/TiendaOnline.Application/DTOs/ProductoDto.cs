namespace TiendaOnline.Application.DTOs;

/// <summary>
/// Datos de un producto que viajan por la API hacia el frontend.
/// Se usa un DTO para no exponer nunca la entidad de dominio.
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
    string Imagen);