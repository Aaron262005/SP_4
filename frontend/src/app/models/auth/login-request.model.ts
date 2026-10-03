/** Datos que se envían a POST /auth/login. */
export interface LoginRequest {
  nombreUsuario: string;
  contrasena: string;
}