using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Api.Filters;

/// <summary>Autoriza solo las escrituras de inventario sin cambiar los endpoints existentes.</summary>
public class AdministradorProductosFilter(IValidadorToken tokens, IUsuarioRepository usuarios) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var cabecera = context.HttpContext.Request.Headers.Authorization.ToString();
        var id = cabecera.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? tokens.ObtenerUsuarioId(cabecera[7..].Trim()) : null;
        if (id is null || await usuarios.ObtenerPorIdAsync(id.Value) is null)
        {
            context.Result = new UnauthorizedObjectResult(new { mensaje = "Inicia sesión con un token válido y vigente." });
            return;
        }
        // La regla escolar se resuelve en el servidor a partir de la identidad firmada.
        if (id != 1 && id != 2)
            context.Result = new ObjectResult(new { mensaje = "Solo un Administrador puede modificar productos." }) { StatusCode = 403 };
    }
}
