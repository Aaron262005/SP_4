import { Injectable } from '@angular/core';
import { Rol } from '../../models/auth/rol.model';

// Reglas de negocio de la US01, definidas como constantes para que sean fáciles de ubicar y cambiar.
const IDS_ADMINISTRADOR = [1, 2];
const ID_AUDITOR = 3;

/**
 * ASIGNACIÓN LOCAL DE PERFILES (US01).
 * El mapeo de roles se programa en el código de la aplicación:
 *   - IDs 1 y 2 => Administrador
 *   - ID 3      => Auditor
 *   - Los demás => Cliente
 */
@Injectable({ providedIn: 'root' })
export class RolAsignadorService {
  asignarRol(usuarioId: number): Rol {
    if (IDS_ADMINISTRADOR.includes(usuarioId)) return 'Administrador';
    if (usuarioId === ID_AUDITOR) return 'Auditor';
    return 'Cliente';
  }
}