import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SessionService } from '../../services/auth/session.service'; // Ajusta la ruta si es necesario

// Guard que protege la ruta verificando si el rol es Administrador o Auditor
export const adminAuditorGuard: CanActivateFn = () => {
  const sessionService = inject(SessionService);
  const router = inject(Router);
  
  const rol = sessionService.rol();
  if (rol === 'Administrador' || rol === 'Auditor') {
    return true;
  }
  
  // Si es Cliente o no hay rol, lo expulsa al inicio
  router.navigate(['/inicio']);
  return false;
};