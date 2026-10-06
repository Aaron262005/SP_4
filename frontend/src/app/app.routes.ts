import { Routes } from '@angular/router';
import { authGuard, invitadoGuard } from './core/guards/auth.guard';
import { adminAuditorGuard } from './core/guards/admin-auditor.guard';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'inicio' },
  {
    path: 'login',
    canActivate: [invitadoGuard],
    loadComponent: () => import('./views/auth/login.view').then((m) => m.LoginView),
  },
  {
    path: 'inicio',
    canActivate: [authGuard],
    loadComponent: () => import('./views/inicio/inicio.view').then((m) => m.InicioView),
  },
  // ===== US12 =====
  { 
    path: 'historial-carritos', 
    canActivate: [adminAuditorGuard],
    loadComponent: () => import('./views/historial-carritos/historial-carritos.view').then(m => m.HistorialCarritosView) 
  },
  // ================
  { path: '**', redirectTo: 'inicio' },
];