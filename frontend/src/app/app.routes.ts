import { Routes } from '@angular/router';
import { authGuard, invitadoGuard } from './core/guards/auth.guard';
import { administradorProductosGuard } from './core/guards/administrador-productos.guard';

/**
 * Rutas de la aplicación. Cada pantalla se carga con LAZY LOADING (loadComponent):
 * su código solo se descarga cuando el usuario entra a esa ruta.
 */
export const routes: Routes = [
  // Al abrir la app se va a /inicio; si no hay sesión, el guard lo manda al Login.
  { path: '', pathMatch: 'full', redirectTo: 'inicio' },

  // ===== US01 y US02: sesión =====
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

  // ===== US03, US06, US07 y US08: productos =====
  // US03: /catalogo se conserva como acceso directo y redirige al catálogo principal,
  // para no tener dos rutas que carguen la misma pantalla.
  { path: 'catalogo', pathMatch: 'full', redirectTo: 'productos' },

  // Catálogo (US03): lista de productos; requiere sesión.
  {
    path: 'productos',
    canActivate: [authGuard],
    loadComponent: () => import('./views/productos/catalogo.view').then((m) => m.CatalogoView),
  },
  // US06: formulario para agregar un producto; solo administradores.
  // IMPORTANTE: va antes de 'productos/:id' para que "nuevo" no se tome como un ID.
  {
    path: 'productos/nuevo',
    canActivate: [authGuard, administradorProductosGuard],
    loadComponent: () => import('./views/productos/formulario-producto.view').then((m) => m.FormularioProductoView),
  },
  // US07: formulario para editar un producto; solo administradores.
  {
    path: 'productos/:id/editar',
    canActivate: [authGuard, administradorProductosGuard],
    loadComponent: () => import('./views/productos/formulario-producto.view').then((m) => m.FormularioProductoView),
  },
  // Detalle de un producto; requiere sesión.
  {
    path: 'productos/:id',
    canActivate: [authGuard],
    loadComponent: () => import('./views/productos/detalle-producto.view').then((m) => m.DetalleProductoView),
  },

  // Cualquier ruta desconocida vuelve al inicio.
  { path: '**', redirectTo: 'inicio' },
];