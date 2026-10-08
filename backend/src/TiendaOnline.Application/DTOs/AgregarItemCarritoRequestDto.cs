using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Application.DTOs.Carrito;

/// <summary>
/// DTO con DataAnnotations para validar la petición HTTP POST de añadir al carrito.
/// </summary>
public class AgregarItemCarritoRequestDto
{
    [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "El ID de producto es obligatorio.")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    public string NombreProducto { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
    public decimal PrecioUnitario { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser de al menos 1.")]
    public int Cantidad { get; set; }
}