using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TiendaOnline.Api.Swagger;

/// <summary>Agrega Authorize a Swagger sin reemplazar la configuración original.</summary>
public class ProductosSwagger : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Pega el token obtenido en POST /auth/login. Solo Administrador puede escribir productos."
        });
        options.DocumentFilter<SeguridadProductosSwagger>();
    }
}

/// <summary>Marca únicamente las operaciones protegidas del módulo de productos.</summary>
public class SeguridadProductosSwagger : IDocumentFilter
{
    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        foreach (var ruta in document.Paths.Where(p => p.Key == "/products" || p.Key == "/products/{id}"))
        {
            if (ruta.Value.Operations is null) continue;
            foreach (var operacion in ruta.Value.Operations)
            {
                if (operacion.Key.ToString().ToUpperInvariant() is not ("POST" or "PUT" or "DELETE")) continue;
                operacion.Value.Security = new List<OpenApiSecurityRequirement>
                {
                    new() { [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>() }
                };
            }
        }
    }
}
