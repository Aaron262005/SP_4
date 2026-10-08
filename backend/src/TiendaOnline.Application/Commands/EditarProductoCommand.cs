namespace TiendaOnline.Application.Commands;
/// <summary>Intención de EditarProducto; el handler ejecuta el caso de uso.</summary>
public record EditarProductoCommand(int Id, string Titulo, decimal Precio, string Descripcion, string Categoria, string Imagen);
