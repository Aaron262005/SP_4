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
}