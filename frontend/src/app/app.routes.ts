// ===== US06-US07-US08 =====
import { administradorProductosGuard } from './core/guards/administrador-productos.guard';
// ===== FIN US06-US07-US08 =====
import { Routes } from '@angular/router';
import { authGuard, invitadoGuard } from './core/guards/auth.guard';

/**
 * Rutas de la aplicación. Cada pantalla se carga con LAZY LOADING (loadComponent):
 * su código solo se descarga cuando el usuario entra a esa ruta.
 */
export const routes: Routes = [
  // Al abrir la app se va a /inicio; si no hay sesión, el guard lo manda al Login.
  { path: '', pathMatch: 'full', redirectTo: 'inicio' },

  {
    path: 'login',
    canActivate: [invitadoGuard], // si ya hay sesión, no muestra el Login
    loadComponent: () => import('./views/auth/login.view').then((m) => m.LoginView),
  },
  {
    path: 'inicio',
    canActivate: [authGuard], // ruta protegida: requiere sesión
    loadComponent: () => import('./views/inicio/inicio.view').then((m) => m.InicioView),
  },

  // ===== US06-US07-US08 =====
  { path: 'productos', canActivate: [authGuard], loadComponent: () => import('./views/productos/catalogo.view').then(m => m.CatalogoView) },
  { path: 'productos/nuevo', canActivate: [authGuard, administradorProductosGuard], loadComponent: () => import('./views/productos/formulario-producto.view').then(m => m.FormularioProductoView) },
  { path: 'productos/:id', canActivate: [authGuard], loadComponent: () => import('./views/productos/detalle-producto.view').then(m => m.DetalleProductoView) },
  // ===== FIN US06-US07-US08 =====

  // Cualquier ruta desconocida vuelve al inicio.
  { path: '**', redirectTo: 'inicio' },
];