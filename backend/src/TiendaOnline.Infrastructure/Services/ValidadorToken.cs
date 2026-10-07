using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TiendaOnline.Application.Interfaces;
namespace TiendaOnline.Infrastructure.Services;

/// <summary>Valida los tokens HS256 emitidos por el TokenService escolar existente sin modificar el login.</summary>
public class ValidadorToken : IValidadorToken
{
    // Debe coincidir con TokenService. Esta clave de demostración no es adecuada para producción.
    private const string Clave = "clave-secreta-solo-para-el-trabajo-de-clase-tienda-online-2026";
    public int? ObtenerUsuarioId(string token)
    {
        try
        {
            if (token.Length > 8192) return null;
            var partes = token.Split('.');
            if (partes.Length != 3) return null;
            using var encabezado = JsonDocument.Parse(Decodificar(partes[0]));
            if (encabezado.RootElement.GetProperty("alg").GetString() != "HS256") return null;
            var esperada = HMACSHA256.HashData(Encoding.UTF8.GetBytes(Clave), Encoding.UTF8.GetBytes(partes[0] + "." + partes[1]));
            if (!CryptographicOperations.FixedTimeEquals(esperada, Decodificar(partes[2]))) return null;
            using var carga = JsonDocument.Parse(Decodificar(partes[1]));
            if (carga.RootElement.GetProperty("exp").GetInt64() <= DateTimeOffset.UtcNow.ToUnixTimeSeconds()) return null;
            return int.TryParse(carga.RootElement.GetProperty("sub").GetString(), out var id) && id > 0 ? id : null;
        }
        catch (Exception ex) when (ex is FormatException or JsonException or KeyNotFoundException or InvalidOperationException or OverflowException)
        {
            return null;
        }
    }
    private static byte[] Decodificar(string valor)
    {
        var base64 = valor.Replace('-', '+').Replace('_', '/');
        return Convert.FromBase64String(base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '='));
    }
}
