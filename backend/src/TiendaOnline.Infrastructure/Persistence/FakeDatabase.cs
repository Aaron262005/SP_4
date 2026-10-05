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
        // ===== US03 =====
    /// <summary>"Tabla" de productos con datos de prueba (US03).</summary>
    public List<Producto> Productos { get; } = new()
    {
        // Id, título, precio, descripción, categoría, imagen (URL)
        new Producto(1, "Audífonos inalámbricos", 59.90m, "Audífonos con Bluetooth y 20 horas de batería.", "Electrónica", "https://placehold.co/400x400/png?text=Audifonos"),
        new Producto(2, "Teclado mecánico", 79.99m, "Teclado compacto con interruptores táctiles.", "Electrónica", "https://placehold.co/400x400/png?text=Teclado"),
        new Producto(3, "Collar de plata", 45.50m, "Collar de plata con dije sencillo.", "Joyería", "https://placehold.co/400x400/png?text=Collar"),
        new Producto(4, "Anillo de oro", 120.00m, "Anillo de oro de 14 quilates.", "Joyería", "https://placehold.co/400x400/png?text=Anillo"),
        new Producto(5, "Chamarra de mezclilla", 39.95m, "Chamarra clásica para toda ocasión.", "Ropa de hombre", "https://placehold.co/400x400/png?text=Chamarra"),
        new Producto(6, "Camisa casual", 22.30m, "Camisa de algodón de manga larga.", "Ropa de hombre", "https://placehold.co/400x400/png?text=Camisa"),
        new Producto(7, "Vestido de verano", 34.50m, "Vestido ligero y fresco.", "Ropa de mujer", "https://placehold.co/400x400/png?text=Vestido"),
        new Producto(8, "Blusa de algodón", 18.99m, "Blusa cómoda de uso diario.", "Ropa de mujer", "https://placehold.co/400x400/png?text=Blusa"),
    };
    // ===== FIN US03 =====
}