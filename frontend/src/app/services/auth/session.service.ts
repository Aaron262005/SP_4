import { Injectable, computed, signal } from '@angular/core';
import { Rol } from '../../models/auth/rol.model';
import { Sesion } from '../../models/auth/sesion.model';

// Claves con las que se guardan los datos en el almacenamiento del navegador (localStorage).
const CLAVE_TOKEN = 'tienda.token';
const CLAVE_USUARIO_ID = 'tienda.usuarioId';
const CLAVE_ROL = 'tienda.rol';
const CLAVE_NOMBRE = 'tienda.nombre';

// --- Funciones auxiliares para leer el almacenamiento de forma segura ---
function leerNumero(clave: string): number | null {
  const valor = localStorage.getItem(clave);
  if (valor === null) return null;
  const numero = Number(valor);
  return Number.isInteger(numero) ? numero : null;
}

function leerRol(): Rol | null {
  const valor = localStorage.getItem(CLAVE_ROL);
  // Solo se acepta un rol válido; cualquier otro valor se ignora.
  return valor === 'Administrador' || valor === 'Cliente' || valor === 'Auditor' ? valor : null;
}

/**
 * Administra la SESIÓN del usuario: la guarda, la expone y la borra.
 * Mantiene los datos en DOS lugares: el almacenamiento persistente (para que la sesión
 * sobreviva al recargar la página) y signals en memoria (para que la interfaz reaccione).
 * Nota: localStorage es la alternativa web al "almacenamiento seguro" móvil,
 * pero no es realmente seguro; sirve para este trabajo de clase.
 */
@Injectable({ providedIn: 'root' })
export class SessionService {
  // Estado interno modificable (privado) ...
  private readonly _token = signal<string | null>(localStorage.getItem(CLAVE_TOKEN));
  private readonly _usuarioId = signal<number | null>(leerNumero(CLAVE_USUARIO_ID));
  private readonly _rol = signal<Rol | null>(leerRol());
  private readonly _nombre = signal<string | null>(localStorage.getItem(CLAVE_NOMBRE));

  // ... y versiones de solo lectura para el resto de la app.
  readonly token = this._token.asReadonly();
  readonly usuarioId = this._usuarioId.asReadonly();
  readonly rol = this._rol.asReadonly();
  readonly nombre = this._nombre.asReadonly();

  /** true cuando existe un token guardado. */
  readonly estaAutenticado = computed(() => this._token() !== null);

  /** Guarda la sesión (login exitoso). */
  guardar(sesion: Sesion): void {
    localStorage.setItem(CLAVE_TOKEN, sesion.token);
    localStorage.setItem(CLAVE_USUARIO_ID, String(sesion.usuarioId));
    localStorage.setItem(CLAVE_ROL, sesion.rol);
    localStorage.setItem(CLAVE_NOMBRE, sesion.nombre);

    this._token.set(sesion.token);
    this._usuarioId.set(sesion.usuarioId);
    this._rol.set(sesion.rol);
    this._nombre.set(sesion.nombre);
  }

  /** LIMPIEZA PROFUNDA (US02): borra token, ID, rol y nombre del almacenamiento Y de la memoria. */
  limpiar(): void {
    localStorage.removeItem(CLAVE_TOKEN);
    localStorage.removeItem(CLAVE_USUARIO_ID);
    localStorage.removeItem(CLAVE_ROL);
    localStorage.removeItem(CLAVE_NOMBRE);

    this._token.set(null);
    this._usuarioId.set(null);
    this._rol.set(null);
    this._nombre.set(null);
  }
}