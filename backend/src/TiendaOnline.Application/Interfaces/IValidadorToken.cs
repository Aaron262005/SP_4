namespace TiendaOnline.Application.Interfaces;
/// <summary>Valida un token y devuelve la identidad firmada, nunca el rol enviado por el navegador.</summary>
public interface IValidadorToken
{
    int? ObtenerUsuarioId(string token);
}
