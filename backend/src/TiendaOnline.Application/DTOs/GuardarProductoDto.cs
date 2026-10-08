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
