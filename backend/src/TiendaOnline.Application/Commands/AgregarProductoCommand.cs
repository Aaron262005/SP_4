namespace TiendaOnline.Application.Commands;
/// <summary>Intención de AgregarProducto; el handler ejecuta el caso de uso.</summary>
public record AgregarProductoCommand(string Titulo, decimal Precio, string Descripcion, string Categoria, string Imagen);
