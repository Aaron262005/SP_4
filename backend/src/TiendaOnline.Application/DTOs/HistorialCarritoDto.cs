namespace TiendaOnline.Application.DTOs;

public record HistorialCarritoDto(int Id, int UsuarioId, DateTime FechaCreacion, List<ItemHistorialDto> Items);
public record ItemHistorialDto(int ProductoId, string TituloProducto, int Cantidad);