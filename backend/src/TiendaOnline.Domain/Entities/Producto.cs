namespace TiendaOnline.Domain.Entities;

/// <summary>
/// Entidad de dominio que representa un producto válido del inventario.
/// Los setters son privados: un producto solo se crea por el constructor,
/// que valida los datos. Los cambios crean otra instancia para evitar
/// mutaciones parciales.
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
        // El identificador debe ser positivo.
        if (id <= 0) throw new ArgumentException("El ID debe ser positivo.");

        // El precio no puede ser negativo (regla que aportó US03).
        if (precio < 0) throw new ArgumentException("El precio no puede ser negativo.");

        // Los campos de texto son obligatorios.
        if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("Completa los campos de texto.");

        // La imagen debe ser una URL absoluta http o https.
        if (!Uri.TryCreate(imagen, UriKind.Absolute, out var url) || (url.Scheme != "http" && url.Scheme != "https"))
            throw new ArgumentException("La imagen debe ser una URL HTTP o HTTPS.");

        // Se guardan los valores sin espacios sobrantes.
        Id = id;
        Titulo = titulo.Trim();
        Precio = precio;
        Descripcion = descripcion.Trim();
        Categoria = categoria.Trim();
        Imagen = imagen.Trim();
    }
}