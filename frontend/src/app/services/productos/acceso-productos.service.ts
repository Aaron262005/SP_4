import { Injectable, computed, inject } from '@angular/core';
import { SessionService } from '../auth/session.service';
import { TokenDecoderService } from '../auth/token-decoder.service';

/** Filtro local de permisos. La comprobación definitiva de la firma ocurre en la API. */
@Injectable({ providedIn: 'root' })
export class AccesoProductosService {
  private readonly sesion = inject(SessionService);
  private readonly decoder = inject(TokenDecoderService);
  readonly esAdministrador = computed(() => {
    const token = this.sesion.token();
    const id = token ? this.decoder.obtenerIdUsuario(token) : null;
    return this.sesion.rol() === 'Administrador' && (id === 1 || id === 2);
  });
  puedeModificar(): boolean {
    if (!this.esAdministrador()) return false;
    try {
      const parte = this.sesion.token()!.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
      const carga = JSON.parse(atob(parte.padEnd(parte.length + (4 - parte.length % 4) % 4, '=')));
      return typeof carga.exp === 'number' && carga.exp * 1000 > Date.now();
    } catch { return false; }
  }
}
