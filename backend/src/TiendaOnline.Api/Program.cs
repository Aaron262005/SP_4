using TiendaOnline.Application.Commands;
using TiendaOnline.Application.DTOs;
using TiendaOnline.Application.Handlers.CommandHandlers;
using TiendaOnline.Application.Handlers.QueryHandlers;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Domain.Interfaces;
using TiendaOnline.Infrastructure.Persistence;
using TiendaOnline.Infrastructure.Repositories;
using TiendaOnline.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Servicios básicos de la API ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   // Swagger: documenta y permite probar los endpoints

// --- CORS: permite que el frontend (Angular en el puerto 4200) llame a esta API ---
builder.Services.AddCors(opciones =>
    opciones.AddPolicy("Frontend", politica =>
        politica.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()));

// --- Inyección de dependencias: aquí se conectan las interfaces con sus implementaciones ---

// BD simulada: Singleton = una sola instancia mientras la API esté encendida.
builder.Services.AddSingleton<FakeDatabase>();

// Repositorios y servicios (Scoped = una instancia por petición HTTP).
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Handlers de CQRS: cada interfaz se asocia a su implementación.

// ===== US01 y US02: login y cierre de sesión =====
builder.Services.AddScoped<ICommandHandler<LoginCommand, LoginResponseDto?>, LoginCommandHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerUsuarioPorIdQuery, UsuarioDto?>, ObtenerUsuarioPorIdQueryHandler>();

// ===== US03, US06, US07 y US08: productos =====
// Documentación de Swagger para los endpoints de productos.
builder.Services.AddTransient<Microsoft.Extensions.Options.IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>, TiendaOnline.Api.Swagger.ProductosSwagger>();

// El repositorio debe ser Singleton: su candado y su contador de IDs viven en la instancia.
builder.Services.AddSingleton<IProductoRepository, ProductoRepository>();

// Validación del token y filtro que restringe agregar, editar y eliminar a administradores.
builder.Services.AddScoped<IValidadorToken, ValidadorToken>();
builder.Services.AddScoped<TiendaOnline.Api.Filters.AdministradorProductosFilter>();

// Consultas: catálogo (US03) y detalle.
builder.Services.AddScoped<IQueryHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>>, ObtenerProductosQueryHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerProductoPorIdQuery, ProductoDto?>, ObtenerProductoPorIdQueryHandler>();

// Comandos: agregar (US06), editar (US07) y eliminar (US08).
builder.Services.AddScoped<ICommandHandler<AgregarProductoCommand, ProductoDto>, AgregarProductoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<EditarProductoCommand, ProductoDto?>, EditarProductoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<EliminarProductoCommand, ProductoDto?>, EliminarProductoCommandHandler>();

var app = builder.Build();

// Swagger solo en desarrollo.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Se activa la política CORS definida arriba.
app.UseCors("Frontend");

app.MapControllers();
app.Run();