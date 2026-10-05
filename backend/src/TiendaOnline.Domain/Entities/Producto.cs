using System;

namespace TiendaOnline.Domain.Entities;

/// <summary>
/// Entidad de dominio que representa un artículo de la tienda.
/// Los setters son privados: un producto solo se crea por el constructor,
/// que valida los datos para que nunca exista un producto inválido.
/// </summary>
public class Producto
{
    public int Id { get; private set; }
    public string Titulo { get; private set; }
    public decimal Precio { get; private set; }
    public string Descripcion { get; private set; }
    public string Categoria { get; private set; }
    public string Imagen { get; private set; }

    public Producto(int id, string titulo, decimal precio, string descripcion, string categoria, string imagen)
    {
        // Validaciones de negocio: se hacen aquí para proteger a la entidad.
        if (id <= 0)
            throw new ArgumentException("El identificador debe ser mayor que cero.", nameof(id));
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El título del producto es obligatorio.", nameof(titulo));
        if (precio < 0)
            throw new ArgumentException("El precio no puede ser negativo.", nameof(precio));
        if (string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("La categoría es obligatoria.", nameof(categoria));
        if (string.IsNullOrWhiteSpace(imagen))
            throw new ArgumentException("La imagen es obligatoria.", nameof(imagen));

        Id = id;
        Titulo = titulo;
        Precio = precio;
        Descripcion = descripcion ?? string.Empty;
        Categoria = categoria;
        Imagen = imagen;
    }
}