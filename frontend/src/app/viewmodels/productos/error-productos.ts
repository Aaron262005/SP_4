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
