namespace TiendaOnline.Domain.Entities;

/// <summary>Producto válido del inventario. Los cambios crean otra instancia para evitar mutaciones parciales.</summary>
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
        if (id <= 0) throw new ArgumentException("El ID debe ser positivo.");
        if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("Completa los campos de texto.");
        if (!Uri.TryCreate(imagen, UriKind.Absolute, out var url) || (url.Scheme != "http" && url.Scheme != "https"))
            throw new ArgumentException("La imagen debe ser una URL HTTP o HTTPS.");
        Id = id; Titulo = titulo.Trim(); Precio = precio; Descripcion = descripcion.Trim();
        Categoria = categoria.Trim(); Imagen = imagen.Trim();
    }
}
