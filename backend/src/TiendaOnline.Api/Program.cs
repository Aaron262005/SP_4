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
builder.Services.AddScoped<ICommandHandler<LoginCommand, LoginResponseDto?>, LoginCommandHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerUsuarioPorIdQuery, UsuarioDto?>, ObtenerUsuarioPorIdQueryHandler>();
// ===== US11 =====
builder.Services.AddScoped<TiendaOnline.Domain.Interfaces.IDirectorioRepository, TiendaOnline.Infrastructure.Repositories.DirectorioRepository>();
builder.Services.AddScoped<TiendaOnline.Application.Interfaces.IQueryHandler<TiendaOnline.Application.Queries.ObtenerDirectorioQuery, IEnumerable<TiendaOnline.Application.DTOs.DirectorioUsuarioDto>>, TiendaOnline.Application.Handlers.QueryHandlers.ObtenerDirectorioQueryHandler>();
// ================

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
