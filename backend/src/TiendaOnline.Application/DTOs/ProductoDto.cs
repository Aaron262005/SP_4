using TiendaOnline.Domain.Entities;
namespace TiendaOnline.Application.DTOs;

/// <summary>Respuesta pública de productos; las entidades nunca salen del servidor.</summary>
public record ProductoDto(int Id, string Titulo, decimal Precio, string Descripcion, string Categoria, string Imagen)
{
    public static ProductoDto Desde(Producto p) => new(p.Id, p.Titulo, p.Precio, p.Descripcion, p.Categoria, p.Imagen);
}
