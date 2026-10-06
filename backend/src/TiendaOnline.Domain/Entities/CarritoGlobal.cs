namespace TiendaOnline.Domain.Entities;

/// <summary>Entidades para simular los carritos y el catálogo de productos.</summary>
public class CarritoGlobal
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public List<ItemCarritoGlobal> Items { get; private set; }

    public CarritoGlobal(int id, int usuarioId, DateTime fechaCreacion, List<ItemCarritoGlobal> items)
    {
        Id = id;
        UsuarioId = usuarioId;
        FechaCreacion = fechaCreacion;
        Items = items;
    }
}

public class ItemCarritoGlobal
{
    public int ProductoId { get; private set; }
    public int Cantidad { get; private set; }

    public ItemCarritoGlobal(int productoId, int cantidad) 
    { 
        ProductoId = productoId; 
        Cantidad = cantidad; 
    }
}

public class ProductoGlobal
{
    public int Id { get; private set; }
    public string Titulo { get; private set; }

    public ProductoGlobal(int id, string titulo) 
    { 
        Id = id; 
        Titulo = titulo; 
    }
}