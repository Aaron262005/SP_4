import { Rol } from './rol.model';

/** Datos de la sesión activa que se guardan en el almacenamiento local. */
export interface Sesion {
  token: string;
  usuarioId: number;
  rol: Rol;
  nombre: string;
}