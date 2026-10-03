import { Injectable } from '@angular/core';

/**
 * Decodifica el token recibido del backend para leer el ID del usuario.
 * El token tiene 3 partes separadas por puntos: encabezado.carga.firma
 * El ID viaja en la "carga" (parte 2), en el campo "sub".
 */
@Injectable({ providedIn: 'root' })
export class TokenDecoderService {
  /** Devuelve el ID del usuario, o null si el token no es válido. */
  obtenerIdUsuario(token: string): number | null {
    try {
      const partes = token.split('.');
      if (partes.length !== 3) return null;

      // La carga viene en Base64Url: se convierte a Base64 normal y se completa el relleno "=".
      const base64 = partes[1].replace(/-/g, '+').replace(/_/g, '/');
      const relleno = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=');

      // Se decodifica a texto y luego a objeto JSON.
      const bytes = Uint8Array.from(atob(relleno), (c) => c.charCodeAt(0));
      const carga = JSON.parse(new TextDecoder().decode(bytes));

      const id = Number(carga.sub);
      return Number.isInteger(id) ? id : null;
    } catch {
      // Cualquier error de formato significa que el token no es válido.
      return null;
    }
  }
}