namespace TiendaOnline.Application.Commands.Carrito;

/// <summary>
/// Command CQRS que representa la intención de añadir un artículo al carrito.
/// Objeto inmutable (record).
/// </summary>
public record AgregarItemCarritoCommand(
    int UsuarioId,
    int ProductoId,
    string NombreProducto,
    decimal PrecioUnitario,
    int Cantidad
);