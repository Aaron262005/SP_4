import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { SessionService } from '../../services/auth/session.service';
import { CarritoService } from '../../services/carrito/carrito.service';

/**
 * VIEWMODEL de la pantalla principal.
 * Expone los datos de la sesión y la acción de cerrar sesión (US02).
 */
@Injectable()
export class InicioViewModel {
  private readonly session = inject(SessionService);
  private readonly auth = inject(AuthService);
  private readonly carrito = inject(CarritoService);
  private readonly router = inject(Router);

  // Datos que la vista muestra (vienen de la sesión).
  readonly nombre = this.session.nombre;
  readonly rol = this.session.rol;
  readonly totalCarrito = this.carrito.totalProductos;

  /**
   * US02 - Cierre de sesión:
   *  1. Limpia token, ID, rol y carrito.
   *  2. Redirige al Login con replaceUrl para REEMPLAZAR esta pantalla en el historial,
   *     de modo que el botón "Atrás" no pueda regresar al contenido protegido.
   */
  cerrarSesion(): void {
    this.auth.cerrarSesion();
    this.router.navigate(['/login'], { replaceUrl: true });
  }
}