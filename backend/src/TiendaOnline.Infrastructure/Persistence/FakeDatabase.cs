using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Infrastructure.Persistence;

/// <summary>
/// BASE DE DATOS SIMULADA (en memoria).
/// Esta clase hace el papel de la base de datos: guarda las "tablas" como listas.
/// Si algún día se usa una BD real, solo se reemplaza esta clase y los repositorios;
/// el resto del sistema no cambia.
/// </summary>
public class FakeDatabase
{
    /// <summary>"Tabla" de usuarios con datos de prueba.</summary>
    public List<Usuario> Usuarios { get; } = new()
    {
        // Id, usuario, contraseña, nombre completo, correo
        new Usuario(1, "admin.ana",      "admin123",   "Ana Torres",   "ana.torres@tiendaonline.com"),
        new Usuario(2, "admin.luis",     "admin123",   "Luis Herrera", "luis.herrera@tiendaonline.com"),
        new Usuario(3, "auditor.marta",  "auditor123", "Marta Ríos",   "marta.rios@tiendaonline.com"),
        new Usuario(4, "cliente.carlos", "cliente123", "Carlos Pérez", "carlos.perez@correo.com"),
        new Usuario(5, "cliente.sofia",  "cliente123", "Sofía Lima",   "sofia.lima@correo.com"),
        new Usuario(6, "cliente.diego",  "cliente123", "Diego Mora",   "diego.mora@correo.com"),
        
    };
    // ===== US11 =====
public static List<TiendaOnline.Domain.Entities.PerfilDirectorio> Directorios = new()
{
    new TiendaOnline.Domain.Entities.PerfilDirectorio(1, "Ana Administradora", "ana@tienda.com", "555-0001", new TiendaOnline.Domain.Entities.Direccion("Calle Principal 123", "Ciudad de México", "19.4326, -99.1332")),
    new TiendaOnline.Domain.Entities.PerfilDirectorio(4, "Carlos Cliente", "carlos@gmail.com", "555-0004", new TiendaOnline.Domain.Entities.Direccion("Avenida Siempre Viva 742", "Querétaro", "20.5881, -100.3899")),
    new TiendaOnline.Domain.Entities.PerfilDirectorio(5, "Sofía Cliente", "sofia@hotmail.com", "555-0005", new TiendaOnline.Domain.Entities.Direccion("Boulevard del Sol 45", "Monterrey", "25.6866, -100.3161"))
};
// ================
}