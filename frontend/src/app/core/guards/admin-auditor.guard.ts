import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SessionService } from '../../services/auth/session.service';

export const adminAuditorGuard: CanActivateFn = () => {
  const sessionService = inject(SessionService);
  const router = inject(Router);
  
  const rol = sessionService.rol();
  if (rol === 'Administrador' || rol === 'Auditor') {
    return true;
  }
  
  router.navigate(['/inicio']);
  return false;
};