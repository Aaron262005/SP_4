namespace TiendaOnline.Domain.Entities;

/// <summary>
/// Entidad de dominio: representa a un usuario de la tienda.
/// Es una clase "pura": no depende de ningún otro proyecto (ni de la BD ni de la API).
/// </summary>
public class Usuario
{
    // La contraseña vive en un campo privado: nadie fuera de esta clase puede leerla.
    // Solo se puede comparar con el método ValidarContrasena (encapsulamiento).
    private readonly string _contrasena;

    // "private set" => solo el constructor puede asignar estos valores.
    public int Id { get; private set; }
    public string NombreUsuario { get; private set; }
    public string Nombre { get; private set; }
    public string Correo { get; private set; }

    public Usuario(int id, string nombreUsuario, string contrasena, string nombre, string correo)
    {
        // Reglas del negocio: la entidad no permite crearse en un estado inválido.
        if (id <= 0)
            throw new ArgumentException("El identificador debe ser mayor a cero.", nameof(id));
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(nombreUsuario));
        if (string.IsNullOrWhiteSpace(contrasena))
            throw new ArgumentException("La contraseña es obligatoria.", nameof(contrasena));

        Id = id;
        NombreUsuario = nombreUsuario;
        _contrasena = contrasena;
        Nombre = nombre;
        Correo = correo;
    }

    /// <summary>
    /// Compara la contraseña recibida con la del usuario.
    /// (En un sistema real se guardaría un hash, no el texto plano; aquí es una simulación.)
    /// </summary>
    public bool ValidarContrasena(string contrasena) => _contrasena == contrasena;
}