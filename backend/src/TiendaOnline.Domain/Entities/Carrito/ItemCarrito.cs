namespace TiendaOnline.Domain.Entities.Carrito;

/// <summary>
/// Entidad de dominio que representa un artículo en el carrito de un cliente.
/// Mantiene encapsuladas las reglas de negocio sobre cantidades y precios.
/// </summary>
public class ItemCarrito
{
    // Propiedades con private set: Solo la propia clase puede modificar sus valores (Encapsulamiento / SOLID S)
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public int ProductoId { get; private set; }
    public string NombreProducto { get; private set; } = string.Empty;
    public decimal PrecioUnitario { get; private set; }
    public int Cantidad { get; private set; }

    // Constructor que valida las reglas de negocio del Dominio al instanciar
    public ItemCarrito(int id, int usuarioId, int productoId, string nombreProducto, decimal precioUnitario, int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(cantidad));

        if (precioUnitario < 0)
            throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(precioUnitario));

        Id = id;
        UsuarioId = usuarioId;
        ProductoId = productoId;
        NombreProducto = nombreProducto;
        PrecioUnitario = precioUnitario;
        Cantidad = cantidad;
    }

    /// <summary>
    /// Regla US09 - Escenario 2: Si el producto ya existía en el carrito, se incrementa la cantidad existente.
    /// </summary>
    public void SumarCantidad(int adicional)
    {
        if (adicional <= 0) return;
        Cantidad += adicional;
    }

    /// <summary>
    /// Regla US10: Permite modificar la cantidad de un producto en el carrito.
    /// </summary>
    public void ActualizarCantidad(int nuevaCantidad)
    {
        if (nuevaCantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(nuevaCantidad));

        Cantidad = nuevaCantidad;
    }
}