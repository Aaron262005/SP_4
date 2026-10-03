using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Infrastructure.Services;

/// <summary>
/// Genera un token con formato JWT:  encabezado.carga.firma  (tres partes en Base64Url).
/// Se arma a mano con las librerías de .NET, sin paquetes adicionales.
/// La "carga" lleva el ID del usuario en el campo "sub"; el frontend lo decodifica.
/// </summary>
public class TokenService : ITokenService
{
    // Clave para firmar el token. En un sistema real iría en configuración segura, no en el código.
    private const string ClaveSecreta = "clave-secreta-solo-para-el-trabajo-de-clase-tienda-online-2026";
    private const int MinutosDeVigencia = 60;

    public string GenerarToken(Usuario usuario)
    {
        // Parte 1: encabezado (algoritmo de firma).
        var encabezado = new { alg = "HS256", typ = "JWT" };

        // Parte 2: carga con los datos del usuario y la fecha de expiración.
        var carga = new
        {
            sub = usuario.Id.ToString(),            // "sub" = identificador del usuario
            username = usuario.NombreUsuario,
            exp = DateTimeOffset.UtcNow.AddMinutes(MinutosDeVigencia).ToUnixTimeSeconds()
        };

        var encabezadoB64 = CodificarBase64Url(JsonSerializer.SerializeToUtf8Bytes(encabezado));
        var cargaB64 = CodificarBase64Url(JsonSerializer.SerializeToUtf8Bytes(carga));

        // Parte 3: firma HMAC-SHA256 de "encabezado.carga" con la clave secreta.
        var firmaB64 = CodificarBase64Url(Firmar($"{encabezadoB64}.{cargaB64}"));

        return $"{encabezadoB64}.{cargaB64}.{firmaB64}";
    }

    // Calcula la firma del contenido usando la clave secreta.
    private static byte[] Firmar(string contenido)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ClaveSecreta));
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(contenido));
    }

    // Base64Url: variante de Base64 segura para URLs (sin "=", "+" ni "/").
    private static string CodificarBase64Url(byte[] bytes) =>
        Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}