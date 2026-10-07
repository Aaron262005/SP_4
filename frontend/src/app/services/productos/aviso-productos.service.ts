import { Injectable, signal } from '@angular/core';
/** Conserva una confirmación al navegar de edición a detalle o de eliminación a catálogo. */
@Injectable({ providedIn: 'root' })
export class AvisoProductosService {
  readonly mensaje = signal('');
  mostrar(mensaje: string): void { this.mensaje.set(mensaje); }
  cerrar(): void { this.mensaje.set(''); }
}
