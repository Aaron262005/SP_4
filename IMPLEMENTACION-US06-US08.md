# US06, US07 y US08 — Inventario

## A. Alcance y criterios

Implementación sobre SP4 (`13a65cc`), siguiendo la organización de US11 (`6f1185c`) y US12. Backend por capas con CQRS manual; frontend MVVM con signals, servicios y vistas standalone. Las ramas de referencia se consultaron, no se fusionaron.

### Decisiones de integración

- Se usa exclusivamente la API .NET y `FakeDatabase`. Se adapta la simulación de Fake Store a listas en memoria: crear, editar y eliminar sí cambian el inventario durante la ejecución; reiniciar la API restaura los datos iniciales. No se promete el comportamiento de Fake Store de ignorar las escrituras.
- Se agregan catálogo y detalle mínimos porque no existen en SP4. US03 está en una rama separada: sus nombres de campos se respetan como referencia, pero no se fusiona esa historia. Una futura integración con US03 requiere conciliar los archivos de productos compartidos.
- No se agregan restricciones de precio positivo, unicidad de título, stock ni campos ajenos a las historias. El precio debe ser un decimal presente y válido; cero es válido.
- La autorización de escritura valida firma HS256, expiración y existencia del usuario. IDs 1 y 2 son administradores, como en el proyecto. Respuestas 401 para token ausente/inválido; 403 para Cliente/Auditor. Se agregan filtro y servicio, sin modificar login ni middleware existente.
- El secreto de firma ya está fijo en el TokenService escolar. El validador usa esa misma clave: sirve para la demostración y no constituye una configuración segura de producción. Si se cambia en el futuro, ambos deben migrarse a configuración segura compartida.
- El puerto 5000 se conserva en los archivos originales. En este Mac lo ocupa Control Center; la comprobación local utilizó 5057 y un proxy temporal en 4200 sin cambiar el código entregado.

| Historia / escenario | Implementación principal | Resultado |
|---|---|---|
| US06: alta exitosa | AgregarProductoCommandHandler, ProductosController, FormularioProductoViewModel | POST 201, ID generado, mensaje y formulario limpio |
| US06: datos inválidos | GuardarProductoDto, validadores del formulario | Error local, campos rojos, ninguna petición |
| US06: permisos | administradorProductosGuard, AccesoProductosService, AdministradorProductosFilter | Cliente/Auditor vuelven al catálogo y no escriben |
| US07: actualización | EditarProductoCommandHandler, formulario y detalle | PUT 200, mensaje «Producto actualizado (Simulación)» y detalle actualizado |
| US07: precarga | ObtenerProductoPorIdQueryHandler, FormularioProductoViewModel | Título, precio, descripción, categoría e imagen precargados |
| US07: permisos y duplicados | filtro, servicio, guardando | Acceso restringido y botón bloqueado durante la petición |
| US08: confirmar | EliminarProductoCommandHandler, DetalleProductoView | DELETE 200, aviso de éxito y regreso al catálogo |
| US08: cancelar | diálogo nativo y eliminar(false) | No se envía DELETE ni cambia el producto |
| US08: permisos | guard, servicio, filtro y @if | Botón oculto y escrituras rechazadas en servidor |

## B. Archivos y creación en PowerShell

El ZIP ya incluye todos los archivos. Los comandos siguientes son una referencia para reconstruir únicamente los archivos nuevos desde la raíz del proyecto (no ejecutarlos para vaciar archivos existentes). Las rutas son completas desde esa raíz.

```powershell
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Api/Controllers' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Api/Filters' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Api/Swagger' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Application/Commands' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Application/DTOs' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Application/Handlers/CommandHandlers' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Application/Handlers/QueryHandlers' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Application/Interfaces' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Application/Queries' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Domain/Entities' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Domain/Interfaces' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Infrastructure/Repositories' -Force | Out-Null
New-Item -ItemType Directory -Path 'backend/src/TiendaOnline.Infrastructure/Services' -Force | Out-Null
New-Item -ItemType Directory -Path 'frontend/src/app/core/guards' -Force | Out-Null
New-Item -ItemType Directory -Path 'frontend/src/app/models/productos' -Force | Out-Null
New-Item -ItemType Directory -Path 'frontend/src/app/services/productos' -Force | Out-Null
New-Item -ItemType Directory -Path 'frontend/src/app/viewmodels/productos' -Force | Out-Null
New-Item -ItemType Directory -Path 'frontend/src/app/views/productos' -Force | Out-Null
if (-not (Test-Path 'backend/src/TiendaOnline.Api/Controllers/ProductosController.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Api/Controllers/ProductosController.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Api/Filters/AdministradorProductosFilter.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Api/Filters/AdministradorProductosFilter.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Api/Swagger/ProductosSwagger.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Api/Swagger/ProductosSwagger.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Commands/AgregarProductoCommand.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Commands/AgregarProductoCommand.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Commands/EditarProductoCommand.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Commands/EditarProductoCommand.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Commands/EliminarProductoCommand.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Commands/EliminarProductoCommand.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/DTOs/GuardarProductoDto.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/DTOs/GuardarProductoDto.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/DTOs/ProductoDto.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/DTOs/ProductoDto.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Handlers/CommandHandlers/AgregarProductoCommandHandler.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Handlers/CommandHandlers/AgregarProductoCommandHandler.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Handlers/CommandHandlers/EditarProductoCommandHandler.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Handlers/CommandHandlers/EditarProductoCommandHandler.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Handlers/CommandHandlers/EliminarProductoCommandHandler.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Handlers/CommandHandlers/EliminarProductoCommandHandler.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Handlers/QueryHandlers/ObtenerProductoPorIdQueryHandler.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Handlers/QueryHandlers/ObtenerProductoPorIdQueryHandler.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Handlers/QueryHandlers/ObtenerProductosQueryHandler.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Handlers/QueryHandlers/ObtenerProductosQueryHandler.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Interfaces/IValidadorToken.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Interfaces/IValidadorToken.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Queries/ObtenerProductoPorIdQuery.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Queries/ObtenerProductoPorIdQuery.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Application/Queries/ObtenerProductosQuery.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Application/Queries/ObtenerProductosQuery.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Domain/Entities/Producto.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Domain/Entities/Producto.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Domain/Interfaces/IProductoRepository.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Domain/Interfaces/IProductoRepository.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Infrastructure/Repositories/ProductoRepository.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Infrastructure/Repositories/ProductoRepository.cs' | Out-Null }
if (-not (Test-Path 'backend/src/TiendaOnline.Infrastructure/Services/ValidadorToken.cs')) { New-Item -ItemType File -Path 'backend/src/TiendaOnline.Infrastructure/Services/ValidadorToken.cs' | Out-Null }
if (-not (Test-Path 'frontend/src/app/core/guards/administrador-productos.guard.ts')) { New-Item -ItemType File -Path 'frontend/src/app/core/guards/administrador-productos.guard.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/models/productos/producto.model.ts')) { New-Item -ItemType File -Path 'frontend/src/app/models/productos/producto.model.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/services/productos/acceso-productos.service.ts')) { New-Item -ItemType File -Path 'frontend/src/app/services/productos/acceso-productos.service.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/services/productos/aviso-productos.service.ts')) { New-Item -ItemType File -Path 'frontend/src/app/services/productos/aviso-productos.service.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/services/productos/productos.service.ts')) { New-Item -ItemType File -Path 'frontend/src/app/services/productos/productos.service.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/viewmodels/productos/catalogo.viewmodel.ts')) { New-Item -ItemType File -Path 'frontend/src/app/viewmodels/productos/catalogo.viewmodel.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/viewmodels/productos/detalle-producto.viewmodel.ts')) { New-Item -ItemType File -Path 'frontend/src/app/viewmodels/productos/detalle-producto.viewmodel.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/viewmodels/productos/error-productos.ts')) { New-Item -ItemType File -Path 'frontend/src/app/viewmodels/productos/error-productos.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/viewmodels/productos/formulario-producto.viewmodel.ts')) { New-Item -ItemType File -Path 'frontend/src/app/viewmodels/productos/formulario-producto.viewmodel.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/viewmodels/productos/productos.spec.ts')) { New-Item -ItemType File -Path 'frontend/src/app/viewmodels/productos/productos.spec.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/_productos.scss')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/_productos.scss' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/catalogo.view.html')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/catalogo.view.html' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/catalogo.view.scss')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/catalogo.view.scss' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/catalogo.view.ts')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/catalogo.view.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/detalle-producto.view.html')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/detalle-producto.view.html' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/detalle-producto.view.scss')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/detalle-producto.view.scss' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/detalle-producto.view.ts')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/detalle-producto.view.ts' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/formulario-producto.view.html')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/formulario-producto.view.html' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/formulario-producto.view.scss')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/formulario-producto.view.scss' | Out-Null }
if (-not (Test-Path 'frontend/src/app/views/productos/formulario-producto.view.ts')) { New-Item -ItemType File -Path 'frontend/src/app/views/productos/formulario-producto.view.ts' | Out-Null }
```

## C. Código completo de archivos nuevos

### `backend/src/TiendaOnline.Api/Controllers/ProductosController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Api.Filters;
using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
namespace TiendaOnline.Api.Controllers;

/// <summary>Adapta HTTP a CQRS. La autorización y persistencia viven fuera del controlador.</summary>
[ApiController]
[Route("products")]
public class ProductosController(
    IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>> listar,
    IQueryHandler<ObtenerProductoPorIdQuery, ProductoDto?> detalle,
    ICommandHandler<AgregarProductoCommand, ProductoDto> agregar,
    ICommandHandler<EditarProductoCommand, ProductoDto?> editar,
    ICommandHandler<EliminarProductoCommand, ProductoDto?> eliminar) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken ct) => Ok(await listar.HandleAsync(new ObtenerProductosQuery(), ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obtener(int id, CancellationToken ct)
    {
        var producto = await detalle.HandleAsync(new ObtenerProductoPorIdQuery(id), ct);
        return producto is null ? NotFound(new { mensaje = "Producto no encontrado." }) : Ok(producto);
    }

    /// <summary>US06: crea un producto y devuelve su ID generado.</summary>
    [HttpPost]
    [ServiceFilter(typeof(AdministradorProductosFilter))]
    public async Task<IActionResult> Agregar(GuardarProductoDto datos, CancellationToken ct)
    {
        var producto = await agregar.HandleAsync(new AgregarProductoCommand(datos.Titulo, datos.Precio!.Value, datos.Descripcion, datos.Categoria, datos.Imagen), ct);
        return CreatedAtAction(nameof(Obtener), new { id = producto.Id }, producto);
    }

    /// <summary>US07: actualiza un producto existente.</summary>
    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(AdministradorProductosFilter))]
    public async Task<IActionResult> Editar(int id, GuardarProductoDto datos, CancellationToken ct)
    {
        var producto = await editar.HandleAsync(new EditarProductoCommand(id, datos.Titulo, datos.Precio!.Value, datos.Descripcion, datos.Categoria, datos.Imagen), ct);
        return producto is null ? NotFound(new { mensaje = "Producto no encontrado." }) : Ok(producto);
    }

    /// <summary>US08: devuelve el objeto eliminado para confirmar el resultado.</summary>
    [HttpDelete("{id:int}")]
    [ServiceFilter(typeof(AdministradorProductosFilter))]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        var producto = await eliminar.HandleAsync(new EliminarProductoCommand(id), ct);
        return producto is null ? NotFound(new { mensaje = "Producto no encontrado." }) : Ok(producto);
    }
}
```

### `backend/src/TiendaOnline.Api/Filters/AdministradorProductosFilter.cs`

```csharp
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
```

### `backend/src/TiendaOnline.Api/Swagger/ProductosSwagger.cs`

```csharp
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
```

### `backend/src/TiendaOnline.Application/Commands/AgregarProductoCommand.cs`

```csharp
namespace TiendaOnline.Application.Commands;
/// <summary>Intención de AgregarProducto; el handler ejecuta el caso de uso.</summary>
public record AgregarProductoCommand(string Titulo, decimal Precio, string Descripcion, string Categoria, string Imagen);
```

### `backend/src/TiendaOnline.Application/Commands/EditarProductoCommand.cs`

```csharp
namespace TiendaOnline.Application.Commands;
/// <summary>Intención de EditarProducto; el handler ejecuta el caso de uso.</summary>
public record EditarProductoCommand(int Id, string Titulo, decimal Precio, string Descripcion, string Categoria, string Imagen);
```

### `backend/src/TiendaOnline.Application/Commands/EliminarProductoCommand.cs`

```csharp
namespace TiendaOnline.Application.Commands;
/// <summary>Intención de EliminarProducto; el handler ejecuta el caso de uso.</summary>
public record EliminarProductoCommand(int Id);
```

### `backend/src/TiendaOnline.Application/DTOs/GuardarProductoDto.cs`

```csharp
using System.ComponentModel.DataAnnotations;
namespace TiendaOnline.Application.DTOs;

/// <summary>Validación compartida de US06 y US07. Precio nullable distingue un cero de un campo omitido.</summary>
public class GuardarProductoDto : IValidatableObject
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    public string Titulo { get; set; } = string.Empty;
    [Required(ErrorMessage = "El precio es obligatorio y debe ser numérico.")]
    public decimal? Precio { get; set; }
    [Required(ErrorMessage = "La descripción es obligatoria.")]
    public string Descripcion { get; set; } = string.Empty;
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public string Categoria { get; set; } = string.Empty;
    [Required(ErrorMessage = "La URL de imagen es obligatoria.")]
    public string Imagen { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Uri.TryCreate(Imagen, UriKind.Absolute, out var uri) || (uri.Scheme != "http" && uri.Scheme != "https"))
            yield return new ValidationResult("Usa una URL HTTP o HTTPS válida.", new[] { nameof(Imagen) });
    }
}
```

### `backend/src/TiendaOnline.Application/DTOs/ProductoDto.cs`

```csharp
using TiendaOnline.Domain.Entities;
namespace TiendaOnline.Application.DTOs;

/// <summary>Respuesta pública de productos; las entidades nunca salen del servidor.</summary>
public record ProductoDto(int Id, string Titulo, decimal Precio, string Descripcion, string Categoria, string Imagen)
{
    public static ProductoDto Desde(Producto p) => new(p.Id, p.Titulo, p.Precio, p.Descripcion, p.Categoria, p.Imagen);
}
```

### `backend/src/TiendaOnline.Application/Handlers/CommandHandlers/AgregarProductoCommandHandler.cs`

```csharp
using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.CommandHandlers;

/// <summary>Ejecuta AgregarProducto mediante el contrato del repositorio (CQRS y SOLID).</summary>
public class AgregarProductoCommandHandler(IProductoRepository productos) : ICommandHandler<AgregarProductoCommand, ProductoDto>
{
    public async Task<ProductoDto> HandleAsync(AgregarProductoCommand command, CancellationToken cancellationToken = default)
    {
        var producto = await productos.AgregarAsync(command.Titulo, command.Precio, command.Descripcion, command.Categoria, command.Imagen, cancellationToken);
        return ProductoDto.Desde(producto);
    }
}
```

### `backend/src/TiendaOnline.Application/Handlers/CommandHandlers/EditarProductoCommandHandler.cs`

```csharp
using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.CommandHandlers;

/// <summary>Ejecuta EditarProducto mediante el contrato del repositorio (CQRS y SOLID).</summary>
public class EditarProductoCommandHandler(IProductoRepository productos) : ICommandHandler<EditarProductoCommand, ProductoDto?>
{
    public async Task<ProductoDto?> HandleAsync(EditarProductoCommand command, CancellationToken cancellationToken = default)
    {
        var producto = await productos.ActualizarAsync(command.Id, command.Titulo, command.Precio, command.Descripcion, command.Categoria, command.Imagen, cancellationToken);
        return producto is null ? null : ProductoDto.Desde(producto);
    }
}
```

### `backend/src/TiendaOnline.Application/Handlers/CommandHandlers/EliminarProductoCommandHandler.cs`

```csharp
using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.CommandHandlers;

/// <summary>Ejecuta EliminarProducto mediante el contrato del repositorio (CQRS y SOLID).</summary>
public class EliminarProductoCommandHandler(IProductoRepository productos) : ICommandHandler<EliminarProductoCommand, ProductoDto?>
{
    public async Task<ProductoDto?> HandleAsync(EliminarProductoCommand command, CancellationToken cancellationToken = default)
    {
        var producto = await productos.EliminarAsync(command.Id, cancellationToken);
        return producto is null ? null : ProductoDto.Desde(producto);
    }
}
```

### `backend/src/TiendaOnline.Application/Handlers/QueryHandlers/ObtenerProductoPorIdQueryHandler.cs`

```csharp
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>Consulta los productos y transforma las entidades a DTO.</summary>
public class ObtenerProductoPorIdQueryHandler(IProductoRepository productos) : IQueryHandler<ObtenerProductoPorIdQuery, ProductoDto?>
{
    public async Task<ProductoDto?> HandleAsync(ObtenerProductoPorIdQuery query, CancellationToken cancellationToken = default)
    {
        var producto = await productos.ObtenerPorIdAsync(query.Id, cancellationToken);
        return producto is null ? null : ProductoDto.Desde(producto);
    }
}
```

### `backend/src/TiendaOnline.Application/Handlers/QueryHandlers/ObtenerProductosQueryHandler.cs`

```csharp
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;
namespace TiendaOnline.Application.Handlers.QueryHandlers;

/// <summary>Consulta los productos y transforma las entidades a DTO.</summary>
public class ObtenerProductosQueryHandler(IProductoRepository productos) : IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>>
{
    public async Task<IReadOnlyList<ProductoDto>> HandleAsync(ObtenerProductosQuery query, CancellationToken cancellationToken = default)
    {
        return (await productos.ObtenerTodosAsync(cancellationToken)).Select(ProductoDto.Desde).ToArray();
    }
}
```

### `backend/src/TiendaOnline.Application/Interfaces/IValidadorToken.cs`

```csharp
namespace TiendaOnline.Application.Interfaces;
/// <summary>Valida un token y devuelve la identidad firmada, nunca el rol enviado por el navegador.</summary>
public interface IValidadorToken
{
    int? ObtenerUsuarioId(string token);
}
```

### `backend/src/TiendaOnline.Application/Queries/ObtenerProductoPorIdQuery.cs`

```csharp
namespace TiendaOnline.Application.Queries;
/// <summary>Consulta de lectura para el catálogo y el detalle.</summary>
public record ObtenerProductoPorIdQuery(int Id);
```

### `backend/src/TiendaOnline.Application/Queries/ObtenerProductosQuery.cs`

```csharp
namespace TiendaOnline.Application.Queries;
/// <summary>Consulta de lectura para el catálogo y el detalle.</summary>
public record ObtenerProductosQuery();
```

### `backend/src/TiendaOnline.Domain/Entities/Producto.cs`

```csharp
namespace TiendaOnline.Domain.Entities;

/// <summary>Producto válido del inventario. Los cambios crean otra instancia para evitar mutaciones parciales.</summary>
public class Producto
{
    public int Id { get; private set; }
    public string Titulo { get; private set; }
    public decimal Precio { get; private set; }
    public string Descripcion { get; private set; }
    public string Categoria { get; private set; }
    public string Imagen { get; private set; }

    public Producto(int id, string titulo, decimal precio, string descripcion, string categoria, string imagen)
    {
        if (id <= 0) throw new ArgumentException("El ID debe ser positivo.");
        if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("Completa los campos de texto.");
        if (!Uri.TryCreate(imagen, UriKind.Absolute, out var url) || (url.Scheme != "http" && url.Scheme != "https"))
            throw new ArgumentException("La imagen debe ser una URL HTTP o HTTPS.");
        Id = id; Titulo = titulo.Trim(); Precio = precio; Descripcion = descripcion.Trim();
        Categoria = categoria.Trim(); Imagen = imagen.Trim();
    }
}
```

### `backend/src/TiendaOnline.Domain/Interfaces/IProductoRepository.cs`

```csharp
using TiendaOnline.Domain.Entities;
namespace TiendaOnline.Domain.Interfaces;

/// <summary>Contrato compartido del inventario, independiente de la persistencia.</summary>
public interface IProductoRepository
{
    Task<IReadOnlyList<Producto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Producto> AgregarAsync(string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default);
    Task<Producto?> ActualizarAsync(int id, string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default);
    Task<Producto?> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
```

### `backend/src/TiendaOnline.Infrastructure/Repositories/ProductoRepository.cs`

```csharp
using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;
namespace TiendaOnline.Infrastructure.Repositories;

/// <summary>Inventario en memoria. Singleton y bloqueo hacen atómicas las escrituras y la asignación de IDs.</summary>
public class ProductoRepository(FakeDatabase db) : IProductoRepository
{
    private readonly object _candado = new();
    private int _ultimoId = db.Productos.Select(p => p.Id).DefaultIfEmpty(0).Max();

    public Task<IReadOnlyList<Producto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado) return Task.FromResult<IReadOnlyList<Producto>>(db.Productos.ToArray());
    }
    public Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado) return Task.FromResult(db.Productos.FirstOrDefault(p => p.Id == id));
    }
    public Task<Producto> AgregarAsync(string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado)
        {
            var producto = new Producto(checked(_ultimoId + 1), titulo, precio, descripcion, categoria, imagen);
            db.Productos.Add(producto);
            _ultimoId = producto.Id;
            return Task.FromResult(producto);
        }
    }
    public Task<Producto?> ActualizarAsync(int id, string titulo, decimal precio, string descripcion, string categoria, string imagen, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado)
        {
            var indice = db.Productos.FindIndex(p => p.Id == id);
            if (indice < 0) return Task.FromResult<Producto?>(null);
            var producto = new Producto(id, titulo, precio, descripcion, categoria, imagen);
            db.Productos[indice] = producto;
            return Task.FromResult<Producto?>(producto);
        }
    }
    public Task<Producto?> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_candado)
        {
            var producto = db.Productos.FirstOrDefault(p => p.Id == id);
            if (producto is not null) db.Productos.Remove(producto);
            return Task.FromResult(producto);
        }
    }
}
```

### `backend/src/TiendaOnline.Infrastructure/Services/ValidadorToken.cs`

```csharp
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
```

### `frontend/src/app/core/guards/administrador-productos.guard.ts`

```typescript
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';

/** Impide que Cliente o Auditor abran alta/edición mediante una URL directa. */
export const administradorProductosGuard: CanActivateFn = () =>
  inject(AccesoProductosService).puedeModificar() || inject(Router).createUrlTree(['/productos']);
```

### `frontend/src/app/models/productos/producto.model.ts`

```typescript
/** Datos compartidos por catálogo, alta, edición y eliminación. */
export interface Producto {
  id: number;
  titulo: string;
  precio: number;
  descripcion: string;
  categoria: string;
  imagen: string;
}
export type GuardarProducto = Omit<Producto, 'id'>;
```

### `frontend/src/app/services/productos/acceso-productos.service.ts`

```typescript
import { Injectable, computed, inject } from '@angular/core';
import { SessionService } from '../auth/session.service';
import { TokenDecoderService } from '../auth/token-decoder.service';

/** Filtro local de permisos. La comprobación definitiva de la firma ocurre en la API. */
@Injectable({ providedIn: 'root' })
export class AccesoProductosService {
  private readonly sesion = inject(SessionService);
  private readonly decoder = inject(TokenDecoderService);
  readonly esAdministrador = computed(() => {
    const token = this.sesion.token();
    const id = token ? this.decoder.obtenerIdUsuario(token) : null;
    return this.sesion.rol() === 'Administrador' && (id === 1 || id === 2);
  });
  puedeModificar(): boolean {
    if (!this.esAdministrador()) return false;
    try {
      const parte = this.sesion.token()!.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
      const carga = JSON.parse(atob(parte.padEnd(parte.length + (4 - parte.length % 4) % 4, '=')));
      return typeof carga.exp === 'number' && carga.exp * 1000 > Date.now();
    } catch { return false; }
  }
}
```

### `frontend/src/app/services/productos/aviso-productos.service.ts`

```typescript
import { Injectable, signal } from '@angular/core';
/** Conserva una confirmación al navegar de edición a detalle o de eliminación a catálogo. */
@Injectable({ providedIn: 'root' })
export class AvisoProductosService {
  readonly mensaje = signal('');
  mostrar(mensaje: string): void { this.mensaje.set(mensaje); }
  cerrar(): void { this.mensaje.set(''); }
}
```

### `frontend/src/app/services/productos/productos.service.ts`

```typescript
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, defer, throwError } from 'rxjs';
import { API_URL } from '../../core/api.config';
import { Producto, GuardarProducto } from '../../models/productos/producto.model';
import { SessionService } from '../auth/session.service';
import { AccesoProductosService } from './acceso-productos.service';

/** Model: comunicación HTTP y bloqueo previo de escrituras sin permisos. No administra la pantalla. */
@Injectable({ providedIn: 'root' })
export class ProductosService {
  private readonly http = inject(HttpClient);
  private readonly sesion = inject(SessionService);
  private readonly acceso = inject(AccesoProductosService);
  listar(): Observable<Producto[]> { return this.http.get<Producto[]>(`${API_URL}/products`); }
  obtener(id: number): Observable<Producto> { return this.http.get<Producto>(`${API_URL}/products/${id}`); }
  agregar(datos: GuardarProducto): Observable<Producto> {
    return this.autorizar(() => this.http.post<Producto>(`${API_URL}/products`, datos, this.opciones()));
  }
  editar(id: number, datos: GuardarProducto): Observable<Producto> {
    return this.autorizar(() => this.http.put<Producto>(`${API_URL}/products/${id}`, datos, this.opciones()));
  }
  eliminar(id: number): Observable<Producto> {
    return this.autorizar(() => this.http.delete<Producto>(`${API_URL}/products/${id}`, this.opciones()));
  }
  private opciones() { return { headers: { Authorization: `Bearer ${this.sesion.token()}` } }; }
  private autorizar(accion: () => Observable<Producto>): Observable<Producto> {
    // Se comprueban los permisos al suscribirse, antes de construir una petición de red.
    return defer(() => this.acceso.puedeModificar() ? accion() : throwError(() => new Error('Solo un Administrador con sesión vigente puede modificar productos.')));
  }
}
```

### `frontend/src/app/viewmodels/productos/catalogo.viewmodel.ts`

```typescript
import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';
import { Producto } from '../../models/productos/producto.model';
import { ProductosService } from '../../services/productos/productos.service';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { mensajeErrorProductos } from './error-productos';

/** Estado del catálogo mínimo necesario para navegar por US06, US07 y US08. */
@Injectable()
export class CatalogoViewModel {
  private readonly productosService = inject(ProductosService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly avisos = inject(AvisoProductosService);
  readonly esAdministrador = inject(AccesoProductosService).esAdministrador;
  readonly productos = signal<Producto[]>([]);
  readonly cargando = signal(false);
  readonly error = signal('');
  readonly mensaje = this.avisos.mensaje;
  cerrarAviso(): void { this.avisos.cerrar(); }
  cargar(): void {
    if (this.cargando()) return;
    this.cargando.set(true); this.error.set('');
    this.productosService.listar().pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.cargando.set(false))).subscribe({
      next: productos => this.productos.set(productos),
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
}
```

### `frontend/src/app/viewmodels/productos/detalle-producto.viewmodel.ts`

```typescript
import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { Producto } from '../../models/productos/producto.model';
import { ProductosService } from '../../services/productos/productos.service';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { mensajeErrorProductos } from './error-productos';

/** Detalle y US08. Recibe la decisión del diálogo sin conocer el navegador ni el DOM. */
@Injectable()
export class DetalleProductoViewModel {
  private readonly servicio = inject(ProductosService);
  private readonly acceso = inject(AccesoProductosService);
  private readonly avisos = inject(AvisoProductosService);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);
  readonly id = Number(inject(ActivatedRoute).snapshot.paramMap.get('id'));
  readonly esAdministrador = this.acceso.esAdministrador;
  readonly producto = signal<Producto | null>(null);
  readonly cargando = signal(false);
  readonly eliminando = signal(false);
  readonly error = signal('');
  readonly mensaje = this.avisos.mensaje;
  cerrarAviso(): void { this.avisos.cerrar(); }
  cargar(): void {
    if (this.cargando()) return;
    this.cargando.set(true); this.error.set('');
    this.servicio.obtener(this.id).pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.cargando.set(false))).subscribe({
      next: producto => this.producto.set(producto),
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
  eliminar(confirmado: boolean): void {
    if (!confirmado || this.eliminando() || !this.producto()) return;
    if (!this.acceso.puedeModificar()) { this.error.set('No tienes permisos para eliminar.'); return; }
    this.eliminando.set(true); this.error.set('');
    this.servicio.eliminar(this.id).pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.eliminando.set(false))).subscribe({
      next: () => {
        this.avisos.mostrar('Producto eliminado correctamente (Simulación).');
        void this.router.navigate(['/productos']);
      },
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
}
```

### `frontend/src/app/viewmodels/productos/error-productos.ts`

```typescript
import { HttpErrorResponse } from '@angular/common/http';
/** Traduce fallos HTTP a mensajes útiles sin exponer detalles internos. */
export function mensajeErrorProductos(error: unknown): string {
  if (error instanceof HttpErrorResponse) {
    if (error.status === 0) return 'No se pudo conectar con el servidor. Revisa la conexión e inténtalo otra vez.';
    if (error.status === 401) return 'Tu sesión no es válida o ha vencido. Vuelve a iniciar sesión.';
    if (error.status === 403) return 'No tienes permisos para modificar productos.';
    if (error.status === 404) return 'El producto ya no existe. Vuelve al catálogo.';
    if (error.status === 400) return 'Revisa los datos: hay campos vacíos o con formato incorrecto.';
    return 'No se pudo completar la operación. Inténtalo otra vez.';
  }
  return error instanceof Error ? error.message : 'Ocurrió un error inesperado.';
}
```

### `frontend/src/app/viewmodels/productos/formulario-producto.viewmodel.ts`

```typescript
import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { ProductosService } from '../../services/productos/productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';
import { mensajeErrorProductos } from './error-productos';

/** Validaciones locales compartidas: nunca se envían formularios inválidos. */
const textoObligatorio: ValidatorFn = control => typeof control.value === 'string' && control.value.trim() ? null : { required: true };
const numeroValido: ValidatorFn = control => typeof control.value === 'number' && Number.isFinite(control.value) ? null : { numero: true };
const urlValida: ValidatorFn = control => {
  try { const url = new URL(control.value); return ['http:', 'https:'].includes(url.protocol) ? null : { url: true }; }
  catch { return { url: true }; }
};

/** US06 y US07 comparten formulario; el ViewModel coordina validación, carga y guardado. */
@Injectable()
export class FormularioProductoViewModel {
  private readonly servicio = inject(ProductosService);
  private readonly acceso = inject(AccesoProductosService);
  private readonly avisos = inject(AvisoProductosService);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);
  readonly id = Number(inject(ActivatedRoute).snapshot.paramMap.get('id')) || null;
  readonly cargando = signal(false);
  readonly guardando = signal(false);
  readonly listo = signal(this.id === null);
  readonly error = signal('');
  readonly mensaje = signal('');
  readonly formulario = new FormGroup({
    titulo: new FormControl('', { nonNullable: true, validators: [textoObligatorio] }),
    precio: new FormControl<number | null>(null, [Validators.required, numeroValido]),
    descripcion: new FormControl('', { nonNullable: true, validators: [textoObligatorio] }),
    categoria: new FormControl('', { nonNullable: true, validators: [textoObligatorio] }),
    imagen: new FormControl('', { nonNullable: true, validators: [textoObligatorio, urlValida] }),
  });
  cargar(): void {
    this.avisos.cerrar();
    if (this.id === null || this.cargando()) return;
    this.cargando.set(true); this.listo.set(false); this.error.set('');
    this.servicio.obtener(this.id).pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.cargando.set(false))).subscribe({
      next: producto => { this.formulario.patchValue(producto); this.listo.set(true); },
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
  invalido(campo: keyof typeof this.formulario.controls): boolean {
    const control = this.formulario.controls[campo];
    return control.invalid && control.touched;
  }
  guardar(): void {
    if (this.guardando() || !this.listo()) return;
    this.error.set(''); this.mensaje.set('');
    if (!this.acceso.puedeModificar()) { void this.router.navigate(['/productos']); return; }
    this.formulario.markAllAsTouched();
    if (this.formulario.invalid) { this.error.set('Corrige los campos marcados antes de guardar.'); return; }
    const valores = this.formulario.getRawValue();
    const datos = { ...valores, precio: valores.precio!, titulo: valores.titulo.trim(), descripcion: valores.descripcion.trim(), categoria: valores.categoria.trim(), imagen: valores.imagen.trim() };
    this.guardando.set(true);
    const peticion = this.id === null ? this.servicio.agregar(datos) : this.servicio.editar(this.id, datos);
    peticion.pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.guardando.set(false))).subscribe({
      next: producto => {
        if (this.id === null) {
          this.mensaje.set(`Producto creado (Simulación). ID generado: ${producto.id}.`);
          this.formulario.reset();
        } else {
          this.avisos.mostrar('Producto actualizado (Simulación)');
          void this.router.navigate(['/productos', producto.id]);
        }
      },
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
}
```

### `frontend/src/app/viewmodels/productos/productos.spec.ts`

```typescript
import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ActivatedRoute, Router, convertToParamMap, provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { SessionService } from '../../services/auth/session.service';
import { ProductosService } from '../../services/productos/productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { FormularioProductoViewModel } from './formulario-producto.viewmodel';
import { DetalleProductoViewModel } from './detalle-producto.viewmodel';
import { CatalogoViewModel } from './catalogo.viewmodel';
import { administradorProductosGuard } from '../../core/guards/administrador-productos.guard';
import { API_URL } from '../../core/api.config';

/** Pruebas de aceptación locales: validación, permisos, cancelación y efectos de cada respuesta HTTP. */
describe('US06-US07-US08 productos', () => {
  let http: HttpTestingController;
  let servicio: ProductosService;
  let sesion: SessionService;
  const producto = { id: 101, titulo: 'Teclado', precio: 25, descripcion: 'Mecánico', categoria: 'Accesorios', imagen: 'https://example.com/a.png' };
  function iniciar(id = 1, rol: 'Administrador' | 'Cliente' | 'Auditor' = 'Administrador', exp = Date.now() / 1000 + 600) {
    const token = 'e30.' + btoa(JSON.stringify({ sub: String(id), exp })) + '.firma';
    sesion.guardar({ token, usuarioId: id, rol, nombre: 'Prueba' });
  }
  function preparar(id: string | null = null) {
    localStorage.clear();
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]),
      FormularioProductoViewModel, DetalleProductoViewModel, CatalogoViewModel,
      { provide: ActivatedRoute, useValue: { snapshot: { paramMap: convertToParamMap(id ? { id } : {}) } } },
    ] });
    http = TestBed.inject(HttpTestingController); servicio = TestBed.inject(ProductosService); sesion = TestBed.inject(SessionService);
    iniciar();
  }
  afterEach(() => { http?.verify(); localStorage.clear(); TestBed.resetTestingModule(); });

  it('US06 bloquea campos vacíos sin enviar HTTP', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel); vm.guardar();
    expect(vm.invalido('titulo')).toBe(true); expect(vm.error()).toContain('Corrige');
    http.expectNone(`${API_URL}/products`);
  });
  it.each(['javascript:alert(1)', 'sin-url', 'ftp://example.com/a.png'])('US06 rechaza URL inválida %s', imagen => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel); vm.formulario.patchValue({ ...producto, imagen }); vm.guardar();
    expect(vm.invalido('imagen')).toBe(true); http.expectNone(`${API_URL}/products`);
  });
  it('US06 rechaza texto de espacios y precio ausente', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel);
    vm.formulario.patchValue({ ...producto, titulo: '   ', precio: null }); vm.guardar();
    expect(vm.invalido('titulo')).toBe(true); expect(vm.invalido('precio')).toBe(true);
  });
  it('US06 rechaza precio no finito', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel);
    vm.formulario.patchValue({ ...producto, precio: NaN }); vm.guardar(); expect(vm.invalido('precio')).toBe(true);
  });
  it('US06 crea una sola vez, envía token, muestra ID y limpia el formulario', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel); vm.formulario.patchValue(producto);
    vm.guardar(); vm.guardar(); const peticion = http.expectOne(`${API_URL}/products`);
    expect(peticion.request.method).toBe('POST'); expect(peticion.request.headers.get('Authorization')).toContain('Bearer ');
    expect(vm.guardando()).toBe(true); peticion.flush({ ...producto, id: 104 }, { status: 201, statusText: 'Created' });
    expect(vm.mensaje()).toContain('104'); expect(vm.formulario.controls.titulo.value).toBe('');
    expect(vm.formulario.controls.titulo.touched).toBe(false); expect(vm.guardando()).toBe(false);
  });
  it('US06 conserva los datos al fallar la API y permite reintentar', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel); vm.formulario.patchValue(producto); vm.guardar();
    http.expectOne(`${API_URL}/products`).flush({}, { status: 500, statusText: 'Error' });
    expect(vm.guardando()).toBe(false); expect(vm.formulario.controls.titulo.value).toBe(producto.titulo);
    vm.guardar(); http.expectOne(`${API_URL}/products`).flush(producto);
  });
  it.each([[4, 'Cliente'], [3, 'Auditor']] as const)('bloquea POST/PUT/DELETE para %s %s antes de la red', (id, rol) => {
    preparar(); iniciar(id, rol); let errores = 0;
    [servicio.agregar(producto), servicio.editar(101, producto), servicio.eliminar(101)].forEach(p => p.subscribe({ error: () => errores++ }));
    expect(errores).toBe(3); http.expectNone(r => r.url.includes('/products'));
  });
  it('no basta cambiar el rol local de un cliente', () => {
    preparar(); iniciar(4, 'Administrador'); let fallo = false;
    servicio.eliminar(101).subscribe({ error: () => fallo = true }); expect(fallo).toBe(true);
  });
  it('no envía escrituras con sesión vencida', () => {
    preparar(); iniciar(1, 'Administrador', 1); let fallo = false;
    servicio.agregar(producto).subscribe({ error: () => fallo = true }); expect(fallo).toBe(true);
  });
  it('redirige a catálogo en enlaces profundos sin permisos', () => {
    preparar(); iniciar(3, 'Auditor');
    const resultado = TestBed.runInInjectionContext(() => administradorProductosGuard({} as never, {} as never));
    expect(String(resultado)).toBe('/productos');
  });
  it('US07 precarga todos los campos y refleja el éxito al regresar al detalle', () => {
    preparar('101'); const vm = TestBed.inject(FormularioProductoViewModel); const router = TestBed.inject(Router);
    const navegar = vi.spyOn(router, 'navigate').mockResolvedValue(true);
    vm.cargar(); http.expectOne(`${API_URL}/products/101`).flush(producto);
    expect(vm.formulario.controls.titulo.value).toBe(producto.titulo); expect(vm.formulario.controls.imagen.value).toBe(producto.imagen);
    vm.formulario.controls.precio.setValue(99); vm.guardar(); vm.guardar();
    const peticion = http.expectOne(`${API_URL}/products/101`); expect(peticion.request.method).toBe('PUT'); expect(peticion.request.body.precio).toBe(99);
    peticion.flush({ ...producto, precio: 99 }); expect(navegar).toHaveBeenCalledWith(['/productos', 101]);
    expect(TestBed.inject(AvisoProductosService).mensaje()).toBe('Producto actualizado (Simulación)');
  });
  it('US07 no permite guardar un producto que no existe', () => {
    preparar('999'); const vm = TestBed.inject(FormularioProductoViewModel); vm.cargar();
    http.expectOne(`${API_URL}/products/999`).flush({}, { status: 404, statusText: 'Not Found' });
    vm.guardar(); expect(vm.listo()).toBe(false); expect(vm.error()).toContain('no existe');
  });
  it('US08 cancelar no envía DELETE ni altera la vista', () => {
    preparar('101'); const vm = TestBed.inject(DetalleProductoViewModel); vm.cargar();
    http.expectOne(`${API_URL}/products/101`).flush(producto); vm.eliminar(false);
    expect(vm.producto()).toEqual(producto); expect(vm.eliminando()).toBe(false); http.expectNone(r => r.method === 'DELETE');
  });
  it('US08 confirmar elimina una vez, conserva aviso y vuelve al catálogo', () => {
    preparar('101'); const vm = TestBed.inject(DetalleProductoViewModel);
    const navegar = vi.spyOn(TestBed.inject(Router), 'navigate').mockResolvedValue(true);
    vm.cargar(); http.expectOne(`${API_URL}/products/101`).flush(producto); vm.eliminar(true); vm.eliminar(true);
    const peticion = http.expectOne(`${API_URL}/products/101`); expect(peticion.request.method).toBe('DELETE'); peticion.flush(producto);
    expect(navegar).toHaveBeenCalledWith(['/productos']); expect(TestBed.inject(AvisoProductosService).mensaje()).toContain('eliminado');
  });
  it('US08 error de borrado conserva el producto y habilita reintento', () => {
    preparar('101'); const vm = TestBed.inject(DetalleProductoViewModel); vm.cargar(); http.expectOne(`${API_URL}/products/101`).flush(producto);
    vm.eliminar(true); http.expectOne(`${API_URL}/products/101`).flush({}, { status: 500, statusText: 'Error' });
    expect(vm.eliminando()).toBe(false); expect(vm.producto()).toEqual(producto); expect(vm.error()).toContain('operación');
  });
  it('catálogo muestra error y se recupera al reintentar', () => {
    preparar(); const vm = TestBed.inject(CatalogoViewModel); vm.cargar(); http.expectOne(`${API_URL}/products`).error(new ProgressEvent('error'));
    expect(vm.error()).toContain('conectar'); expect(vm.cargando()).toBe(false);
    vm.cargar(); http.expectOne(`${API_URL}/products`).flush([producto]); expect(vm.productos()).toEqual([producto]); expect(vm.error()).toBe('');
  });
});
```

### `frontend/src/app/views/productos/_productos.scss`

```scss
// Tarjetas blancas, botones azules y espaciado consistente con US11 y US12.
:host { display: block; color: #263445; }
.productos-container { max-width: 1100px; margin: 0 auto; padding: 24px; }
nav { margin-bottom: 24px; }
header, .acciones { display: flex; align-items: center; justify-content: space-between; gap: 16px; flex-wrap: wrap; }
h1 { font-size: 1.7rem; margin: 0 0 12px; }
h2 { font-size: 1.15rem; overflow-wrap: anywhere; }
p { line-height: 1.6; }
a { color: #0056b3; }
button, .boton { border: 0; border-radius: 6px; padding: 11px 18px; cursor: pointer; font: inherit; }
.boton { background: #0056b3; color: white; text-decoration: none; display: inline-block; }
button:disabled { cursor: wait; opacity: .65; }
:focus-visible { outline: 3px solid #1986cc; outline-offset: 3px; }
.tarjetas-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(min(250px, 100%), 1fr)); gap: 20px; margin-top: 24px; }
.tarjeta { border: 1px solid #ddd; border-radius: 8px; background: white; overflow: hidden; box-shadow: 0 2px 4px #0000000d; }
.tarjeta img { width: 100%; height: 190px; object-fit: contain; background: #f8fafc; }
.contenido { padding: 18px; }
.categoria, .nota { color: #526174; font-size: .9rem; }
.precio { font-size: 1.4rem; font-weight: 700; }
.detalle { display: grid; grid-template-columns: 1fr 1fr; gap: 32px; background: white; border: 1px solid #ddd; padding: 24px; border-radius: 8px; }
.detalle img { width: 100%; max-height: 400px; object-fit: contain; }
.descripcion { white-space: pre-wrap; overflow-wrap: anywhere; }
.acciones { justify-content: flex-start; margin-top: 20px; }
.peligro { background: #b42318; color: white; }
.aviso { background: #e7f7ec; color: #175a30; border: 1px solid #94cea7; border-radius: 6px; padding: 16px; margin: 16px 0; display: flex; justify-content: space-between; gap: 16px; }
.aviso button { padding: 0 8px; background: transparent; color: inherit; }
.formulario { max-width: 720px; }
fieldset { border: 0; margin: 0; padding: 0; min-width: 0; }
.campo { display: grid; gap: 7px; margin: 18px 0; }
label { font-weight: 600; }
input, textarea { box-sizing: border-box; width: 100%; border: 1px solid #8995a5; border-radius: 6px; padding: 12px; font: inherit; }
textarea { resize: vertical; }
.invalido { border: 2px solid #b42318; background: #fff9f8; }
.error-campo { color: #b42318; font-size: .9rem; }
.sr-only { position: absolute; width: 1px; height: 1px; overflow: hidden; clip-path: inset(50%); }
@media (max-width: 640px) { .productos-container { padding: 18px; } .detalle { grid-template-columns: 1fr; padding: 16px; } }
```

### `frontend/src/app/views/productos/catalogo.view.html`

```html
<!-- Catálogo de apoyo: conserva la navegación y muestra los resultados de las tres operaciones. -->
<main class="productos-container">
  <nav><a routerLink="/inicio">← Inicio</a></nav>
  <header><div><h1>Catálogo de productos</h1><p>Inventario de la tienda</p></div>
    @if (vm.esAdministrador()) { <a class="boton" routerLink="/productos/nuevo">Agregar producto</a> }
  </header>
  @if (vm.mensaje()) { <div class="aviso" role="status">{{ vm.mensaje() }} <button type="button" (click)="vm.cerrarAviso()" aria-label="Cerrar confirmación">×</button></div> }
  @if (vm.error()) { <app-alerta [mensaje]="vm.error()" /><button type="button" (click)="vm.cargar()">Reintentar</button> }
  @if (vm.cargando()) { <p role="status">Cargando productos…</p> }
  @if (!vm.cargando() && !vm.error()) {
    <section class="tarjetas-grid" aria-label="Productos">
      @for (producto of vm.productos(); track producto.id) {
        <article class="tarjeta"><img [src]="producto.imagen" [alt]="producto.titulo" loading="lazy" />
          <div class="contenido"><span class="categoria">{{ producto.categoria }}</span><h2>{{ producto.titulo }}</h2>
            <p class="precio">{{ producto.precio | number:'1.2-2' }}</p><a [routerLink]="['/productos', producto.id]">Ver detalle →</a>
          </div>
        </article>
      } @empty { <p>No hay productos en el catálogo.</p> }
    </section>
  }
  <p class="nota">Simulación local: el inventario se reinicia al apagar la API.</p>
</main>
```

### `frontend/src/app/views/productos/catalogo.view.scss`

```scss
// Estilo compartido de las tres historias, sin modificar los estilos existentes.
@use './productos';
```

### `frontend/src/app/views/productos/catalogo.view.ts`

```typescript
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { CatalogoViewModel } from '../../viewmodels/productos/catalogo.viewmodel';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

/** View standalone: presenta el estado y delega las acciones al ViewModel. */
@Component({
  selector: 'app-catalogo', standalone: true,
  imports: [CommonModule, RouterLink, AlertaComponent],
  providers: [CatalogoViewModel],
  templateUrl: './catalogo.view.html', styleUrl: './catalogo.view.scss',
})
export class CatalogoView implements OnInit {
  readonly vm = inject(CatalogoViewModel);
  ngOnInit(): void { this.vm.cargar(); }
}
```

### `frontend/src/app/views/productos/detalle-producto.view.html`

```html
<!-- US07/US08: solo el administrador ve las acciones de edición y eliminación. -->
<main class="productos-container">
  <nav><a routerLink="/productos">← Volver al catálogo</a></nav>
  @if (vm.mensaje()) { <div class="aviso" role="status">{{ vm.mensaje() }} <button type="button" (click)="vm.cerrarAviso()" aria-label="Cerrar confirmación">×</button></div> }
  @if (vm.error()) { <app-alerta [mensaje]="vm.error()" /><button type="button" [disabled]="vm.eliminando()" (click)="vm.cargar()">Reintentar consulta</button> }
  @if (vm.cargando()) { <p role="status">Cargando producto…</p> }
  @if (vm.producto(); as producto) {
    <article class="detalle"><img [src]="producto.imagen" [alt]="producto.titulo" />
      <div><span class="categoria">{{ producto.categoria }} · ID {{ producto.id }}</span>
        <h1>{{ producto.titulo }}</h1><p class="precio">{{ producto.precio | number:'1.2-2' }}</p><p class="descripcion">{{ producto.descripcion }}</p>
        @if (vm.esAdministrador()) {
          <div class="acciones">
            @if (!vm.eliminando()) { <a class="boton" [routerLink]="['/productos', producto.id, 'editar']">Editar</a> }
            <button class="peligro" type="button" [disabled]="vm.eliminando() || vm.cargando()" (click)="confirmarEliminacion()">{{ vm.eliminando() ? 'Eliminando…' : 'Eliminar' }}</button>
          </div>
        }
      </div>
    </article>
  }
</main>
```

### `frontend/src/app/views/productos/detalle-producto.view.scss`

```scss
// Estilo compartido de las tres historias, sin modificar los estilos existentes.
@use './productos';
```

### `frontend/src/app/views/productos/detalle-producto.view.ts`

```typescript
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { DetalleProductoViewModel } from '../../viewmodels/productos/detalle-producto.viewmodel';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

/** View standalone: presenta el estado y delega las acciones al ViewModel. */
@Component({
  selector: 'app-detalle-producto', standalone: true,
  imports: [CommonModule, RouterLink, AlertaComponent],
  providers: [DetalleProductoViewModel],
  templateUrl: './detalle-producto.view.html', styleUrl: './detalle-producto.view.scss',
})
export class DetalleProductoView implements OnInit {
  readonly vm = inject(DetalleProductoViewModel);
  ngOnInit(): void { this.vm.cargar(); }
  confirmarEliminacion(): void {
    // El diálogo nativo pertenece a la View; cancelar no invoca ninguna petición.
    if (!this.vm.eliminando()) this.vm.eliminar(window.confirm('¿Estás seguro de eliminar este producto?'));
  }
}
```

### `frontend/src/app/views/productos/formulario-producto.view.html`

```html
<!-- US06 y US07: formulario accesible, precargado en edición y validado antes de HTTP. -->
<main class="productos-container formulario">
  <nav><a routerLink="/productos">← Volver al catálogo</a></nav>
  <h1>{{ vm.id === null ? 'Agregar producto' : 'Editar producto' }}</h1>
  <p>Completa todos los campos del artículo.</p>
  @if (vm.mensaje()) { <div class="aviso" role="status">{{ vm.mensaje() }}</div> }
  @if (vm.error()) { <app-alerta [mensaje]="vm.error()" /> }
  @if (vm.cargando()) { <p role="status">Cargando información del producto…</p> }
  @if (!vm.listo() && !vm.cargando()) { <button type="button" (click)="vm.cargar()">Reintentar consulta</button> }
  @if (vm.listo()) {
    <form [formGroup]="vm.formulario" (ngSubmit)="vm.guardar()" novalidate>
      <fieldset [disabled]="vm.guardando()" [attr.aria-busy]="vm.guardando()">
        <legend class="sr-only">Información del producto</legend>
        <div class="campo"><label for="titulo">Título</label><input id="titulo" formControlName="titulo" required [class.invalido]="vm.invalido('titulo')" [attr.aria-invalid]="vm.invalido('titulo')" aria-describedby="titulo-error" type="text" />
          @if (vm.invalido('titulo')) { <span class="error-campo" id="titulo-error">Escribe el título del producto.</span> }
        </div>
        <div class="campo"><label for="precio">Precio</label><input id="precio" formControlName="precio" required [class.invalido]="vm.invalido('precio')" [attr.aria-invalid]="vm.invalido('precio')" aria-describedby="precio-error" type="number" step="any" inputmode="decimal" />
          @if (vm.invalido('precio')) { <span class="error-campo" id="precio-error">Ingresa un precio numérico.</span> }
        </div>
        <div class="campo"><label for="descripcion">Descripción</label><textarea id="descripcion" formControlName="descripcion" required [class.invalido]="vm.invalido('descripcion')" [attr.aria-invalid]="vm.invalido('descripcion')" aria-describedby="descripcion-error" rows="4"></textarea>
          @if (vm.invalido('descripcion')) { <span class="error-campo" id="descripcion-error">Escribe una descripción.</span> }
        </div>
        <div class="campo"><label for="categoria">Categoría</label><input id="categoria" formControlName="categoria" required [class.invalido]="vm.invalido('categoria')" [attr.aria-invalid]="vm.invalido('categoria')" aria-describedby="categoria-error" type="text" />
          @if (vm.invalido('categoria')) { <span class="error-campo" id="categoria-error">Escribe la categoría.</span> }
        </div>
        <div class="campo"><label for="imagen">URL de imagen</label><input id="imagen" formControlName="imagen" required [class.invalido]="vm.invalido('imagen')" [attr.aria-invalid]="vm.invalido('imagen')" aria-describedby="imagen-error" type="url" />
          @if (vm.invalido('imagen')) { <span class="error-campo" id="imagen-error">Ingresa una URL válida que empiece con http:// o https://.</span> }
        </div>
        <div class="acciones"><button class="boton" type="submit" [disabled]="vm.guardando()">{{ vm.guardando() ? 'Guardando…' : 'Guardar' }}</button>
          @if (!vm.guardando()) { <a [routerLink]="vm.id === null ? ['/productos'] : ['/productos', vm.id]">Cancelar</a> }
        </div>
      </fieldset>
    </form>
  }
</main>
```

### `frontend/src/app/views/productos/formulario-producto.view.scss`

```scss
// Estilo compartido de las tres historias, sin modificar los estilos existentes.
@use './productos';
```

### `frontend/src/app/views/productos/formulario-producto.view.ts`

```typescript
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormularioProductoViewModel } from '../../viewmodels/productos/formulario-producto.viewmodel';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

/** View standalone: presenta el estado y delega las acciones al ViewModel. */
@Component({
  selector: 'app-formulario-producto', standalone: true,
  imports: [CommonModule, RouterLink, AlertaComponent, ReactiveFormsModule],
  providers: [FormularioProductoViewModel],
  templateUrl: './formulario-producto.view.html', styleUrl: './formulario-producto.view.scss',
})
export class FormularioProductoView implements OnInit {
  readonly vm = inject(FormularioProductoViewModel);
  ngOnInit(): void { this.vm.cargar(); }
}
```

## D. Puntos de integración: únicamente líneas agregadas

### `backend/src/TiendaOnline.Api/Program.cs`

Agregar antes de `var app = builder.Build();`:

```
// ===== US06-US07-US08 =====
builder.Services.AddTransient<Microsoft.Extensions.Options.IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>, TiendaOnline.Api.Swagger.ProductosSwagger>();
builder.Services.AddSingleton<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IValidadorToken, ValidadorToken>();
builder.Services.AddScoped<TiendaOnline.Api.Filters.AdministradorProductosFilter>();
builder.Services.AddScoped<IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>>, ObtenerProductosQueryHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerProductoPorIdQuery, ProductoDto?>, ObtenerProductoPorIdQueryHandler>();
builder.Services.AddScoped<ICommandHandler<AgregarProductoCommand, ProductoDto>, AgregarProductoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<EditarProductoCommand, ProductoDto?>, EditarProductoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<EliminarProductoCommand, ProductoDto?>, EliminarProductoCommandHandler>();
// ===== FIN US06-US07-US08 =====

```

### `backend/src/TiendaOnline.Infrastructure/Persistence/FakeDatabase.cs`

Agregar antes de `}`:

```
    // ===== US06-US07-US08 =====
    /// <summary>Inventario de prueba compartido por las tres historias; se reinicia con la API.</summary>
    public List<Producto> Productos { get; } = new()
    {
        new Producto(101, "MSI Titan 18 HX", 45000, "Laptop para trabajo y juegos.", "Electrónica", "https://placehold.co/640x480/png?text=Laptop"),
        new Producto(102, "Mouse Logitech G Pro", 1200, "Mouse inalámbrico de precisión.", "Accesorios", "https://placehold.co/640x480/png?text=Mouse"),
        new Producto(103, "Teclado Keychron K2", 1800, "Teclado mecánico compacto.", "Accesorios", "https://placehold.co/640x480/png?text=Teclado")
    };
    // ===== FIN US06-US07-US08 =====
```

### `frontend/src/app/app.routes.ts`

Agregar antes de `import { Routes } from '@angular/router';`:

```
// ===== US06-US07-US08 =====
import { administradorProductosGuard } from './core/guards/administrador-productos.guard';
// ===== FIN US06-US07-US08 =====
```

Agregar antes de `// Cualquier ruta desconocida vuelve al inicio.`:

```
  // ===== US06-US07-US08 =====
  { path: 'productos', canActivate: [authGuard], loadComponent: () => import('./views/productos/catalogo.view').then(m => m.CatalogoView) },
  { path: 'productos/nuevo', canActivate: [authGuard, administradorProductosGuard], loadComponent: () => import('./views/productos/formulario-producto.view').then(m => m.FormularioProductoView) },
  { path: 'productos/:id/editar', canActivate: [authGuard, administradorProductosGuard], loadComponent: () => import('./views/productos/formulario-producto.view').then(m => m.FormularioProductoView) },
  { path: 'productos/:id', canActivate: [authGuard], loadComponent: () => import('./views/productos/detalle-producto.view').then(m => m.DetalleProductoView) },
  // ===== FIN US06-US07-US08 =====

```

### `frontend/src/app/views/inicio/inicio.view.html`

Agregar antes de `@switch (vm.rol()) {`:

```
    <!-- ===== US06-US07-US08: acceso al módulo de productos ===== -->
    <p><a href="/productos">Abrir catálogo de productos →</a></p>
    <!-- ===== FIN US06-US07-US08 ===== -->
```

## E. Compilar, ejecutar y probar

Requisitos: SDK .NET 10 y una versión de Node compatible con Angular 22 (se comprobó con Node 24). No se agregaron dependencias ni se modificaron las versiones del proyecto. La solución original se llama `TiendaOnline.slnx`.

Desde la raíz, terminal PowerShell 1:

```powershell
Set-Location backend
dotnet restore
dotnet build
dotnet run --project src/TiendaOnline.Api
```

Terminal PowerShell 2, desde la raíz:

```powershell
Set-Location frontend
npm ci
npm run build
npm start
```

Abrir http://localhost:4200. Entrar con `admin.ana / admin123` o `admin.luis / admin123`, y pulsar «Abrir catálogo de productos». Cliente: `cliente.carlos / cliente123`; auditor: `auditor.marta / auditor123`.

Pruebas nuevas:

```powershell
Set-Location frontend
npm test -- --watch=false --include="**/productos.spec.ts"
```

### Swagger y peticiones

Swagger está en http://localhost:5000/swagger en desarrollo. Ejecutar POST /auth/login, copiar el token y pegarlo en Authorize (sin el prefijo Bearer). Después ejecutar POST, PUT y DELETE con los datos de ejemplo. El botón Authorize se agrega mediante una clase de configuración y un registro nuevo en Program.cs, conservando el AddSwaggerGen original. Las operaciones protegidas están marcadas con candado; consultas y login siguen públicos. Sin token, las escrituras devuelven 401. Alternativa equivalente en PowerShell:

```powershell
$baseUrl = 'http://localhost:5000'
$login = @{ nombreUsuario = 'admin.ana'; contrasena = 'admin123' } | ConvertTo-Json
$sesion = Invoke-RestMethod -Method Post -Uri "$baseUrl/auth/login" -ContentType 'application/json' -Body $login
$cabeceras = @{ Authorization = "Bearer $($sesion.token)" }
$datos = @{ titulo = 'Teclado de prueba'; precio = 250.50; descripcion = 'Prueba de inventario'; categoria = 'Accesorios'; imagen = 'https://example.com/teclado.png' }
$nuevo = Invoke-RestMethod -Method Post -Uri "$baseUrl/products" -Headers $cabeceras -ContentType 'application/json' -Body ($datos | ConvertTo-Json)
$nuevo
$datos.precio = 299
Invoke-RestMethod -Method Put -Uri "$baseUrl/products/$($nuevo.id)" -Headers $cabeceras -ContentType 'application/json' -Body ($datos | ConvertTo-Json)
Invoke-RestMethod -Method Get -Uri "$baseUrl/products/$($nuevo.id)"
Invoke-RestMethod -Method Delete -Uri "$baseUrl/products/$($nuevo.id)" -Headers $cabeceras
```

Esperado: alta 201 con ID nuevo; edición 200 con precio 299; lectura con precio actualizado; eliminación 200 con objeto eliminado; lectura posterior 404. Repetir escrituras con Cliente o Auditor produce 403. Con token modificado o vencido, 401. Título en blanco, precio ausente/no numérico o URL inválida producen 400.

### Casos de interfaz

1. Alta: enviar vacío, comprobar campos rojos; completar datos, guardar y comprobar ID y limpieza.
2. Editar: abrir detalle, pulsar Editar y comprobar valores precargados; cambiar precio, guardar y comprobar el detalle.
3. Eliminar: cancelar el diálogo y comprobar que sigue visible; confirmar y comprobar regreso al catálogo y aviso.
4. Cliente/Auditor: no hay botones de escritura; abrir `/productos/nuevo` o `/productos/101/editar` devuelve al catálogo.
5. Desconectar API: ver mensaje de error; restablecer conexión y reintentar.
6. Simular red lenta: no se permiten dos guardados o borrados en curso.

### Verificación realizada

- `dotnet build`: 0 errores, 0 advertencias.
- Angular: compilación de producción correcta sin advertencias. Este entorno de Mac requirió `CI=true NG_BUILD_PARALLEL_TS=0 NG_BUILD_MAX_WORKERS=1` para evitar un bloqueo de los compiladores paralelos; no fue necesario cambiar el proyecto.
- Pruebas nuevas de Angular: 19/19.
- API: 61 respuestas HTTP verificadas, incluyendo permisos, tokens alterados/vencidos, datos inválidos, CRUD y altas concurrentes con IDs únicos.
- Navegador: login, catálogo, creación con ID, limpieza, precarga, cambio de precio y eliminación con regreso al catálogo. Cancelación verificada por prueba automatizada del ViewModel.
- Suite completa: 20 pasan, 1 falla. El fallo está en `app.spec.ts`, que ya esperaba «Hello, tienda-online» aunque `app.html` solo contiene el router outlet. Ambos archivos originales están intactos.

## F. SOLID, CQRS y MVVM

| Principio | Aplicación |
|---|---|
| S | Cada handler resuelve una acción; repositorio persiste; filtro autoriza; View presenta. |
| O | Se agregan archivos y solo cuatro puntos de integración, todos delimitados. |
| L | ProductoRepository implementa el contrato de productos con los mismos resultados y cancelación. |
| I | IValidadorToken solo valida identidad; los handlers dependen de contratos específicos. |
| D | Application usa IProductoRepository; el filtro usa IValidadorToken. Angular utiliza la inyección de servicios como las historias de referencia. |
| CQRS | Agregar, Editar y Eliminar tienen Command y Handler propios; catálogo y detalle usan Query y Handler. |
| MVVM | Model: tipos y servicio HTTP; ViewModel: signals, formulario y acciones; View: HTML, estilos y diálogo nativo. |

## G. Interfaz

Se conserva el patrón visual de US11/US12: tarjetas claras, bordes suaves y acciones azules. Hay diseño adaptable, etiquetas asociadas a campos, foco visible, estados de carga/error y avisos accesibles. El borrado usa confirmación nativa; el ViewModel recibe solo la decisión.
