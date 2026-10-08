import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SessionService } from '../../services/auth/session.service';

/**
 * Guard compartido por US11 (directorio de usuarios) y US12 (historial de carritos).
 * Protege la ruta verificando que el rol sea Administrador o Auditor.
 */
export const adminAuditorGuard: CanActivateFn = () => {
  const sessionService = inject(SessionService);
  const router = inject(Router);

  // El rol viene de la sesión iniciada.
  const rol = sessionService.rol();
  if (rol === 'Administrador' || rol === 'Auditor') {
    return true;
  }

  // Si es Cliente o no hay rol, lo expulsa al inicio.
  router.navigate(['/inicio']);
  return false;
};