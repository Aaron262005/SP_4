import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SessionService } from '../../services/auth/session.service';

/**
 * Guard para rutas PROTEGIDAS (por ejemplo, /inicio).
 * Si no hay sesión, redirige al Login. Esto impide volver a una pantalla
 * protegida con el botón "Atrás" después de cerrar sesión (US02, escenario 2).
 */
export const authGuard: CanActivateFn = () => {
  const session = inject(SessionService);
  const router = inject(Router);

  // true = deja pasar; UrlTree = redirige a esa ruta.
  return session.estaAutenticado() ? true : router.createUrlTree(['/login']);
};

/**
 * Guard para rutas de INVITADO (el Login).
 * Si ya hay una sesión activa, manda al usuario a la pantalla principal.
 */
export const invitadoGuard: CanActivateFn = () => {
  const session = inject(SessionService);
  const router = inject(Router);

  return session.estaAutenticado() ? router.createUrlTree(['/inicio']) : true;
};