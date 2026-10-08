namespace TiendaOnline.Application.DTOs.Carrito;

/// <summary>
/// DTO de salida para devolver la información formateada de un ítem del carrito.
/// Evita exponer la entidad de Dominio directamente hacia afuera.
/// </summary>
public class ItemCarritoDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int ProductoId { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public decimal PrecioUnitario { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal => Math.Round(PrecioUnitario * Cantidad, 2);
}