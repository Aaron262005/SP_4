namespace TiendaOnline.Domain.Entities;

/// <summary>
/// Entidad que representa la información extendida y anidada de un usuario para el directorio.
/// </summary>
public class PerfilDirectorio
{
    public int UsuarioId { get; private set; }
    public string NombreCompleto { get; private set; }
    public string Correo { get; private set; }
    public string Telefono { get; private set; }
    public Direccion Ubicacion { get; private set; }

    public PerfilDirectorio(int usuarioId, string nombreCompleto, string correo, string telefono, Direccion ubicacion)
    {
        UsuarioId = usuarioId;
        NombreCompleto = nombreCompleto;
        Correo = correo;
        Telefono = telefono;
        Ubicacion = ubicacion;
    }
}

public class Direccion
{
    public string Calle { get; private set; }
    public string Ciudad { get; private set; }
    public string Coordenadas { get; private set; }

    public Direccion(string calle, string ciudad, string coordenadas)
    {
        Calle = calle;
        Ciudad = ciudad;
        Coordenadas = coordenadas;
    }
}