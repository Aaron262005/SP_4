/** Información de un usuario, tal como la devuelve GET /usuarios/{id}. */
export interface Usuario {
  id: number;
  nombreUsuario: string;
  nombre: string;
  correo: string;
}