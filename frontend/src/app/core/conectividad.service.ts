import { Injectable } from '@angular/core';

/**
 * Informa si el dispositivo tiene conexión a internet.
 * Se separa en un servicio para que el ViewModel no dependa directamente del navegador.
 */
@Injectable({ providedIn: 'root' })
export class ConectividadService {
  hayConexion(): boolean {
    // navigator.onLine es false cuando el navegador detecta que no hay red.
    return navigator.onLine;
  }
}