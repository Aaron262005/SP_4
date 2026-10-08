namespace TiendaOnline.Application.DTOs;

/// <summary>
/// DTOs anidados para enviar la información del directorio sin exponer las entidades de Dominio.
/// </summary>
public record DirectorioUsuarioDto(
    int Id, 
    string NombreCompleto, 
    string Correo, 
    string Telefono, 
    DireccionDto Direccion);

public record DireccionDto(
    string Calle, 
    string Ciudad, 
    string Coordenadas);