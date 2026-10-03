import { Injectable, computed, signal } from '@angular/core';
import { ItemCarrito } from '../../models/carrito/item-carrito.model';

/**
 * Estado global del carrito de compras.
 * Aquí solo se deja la base que necesita la US02: poder VACIARLO al cerrar sesión.
 * Agregar y quitar productos pertenece a otras historias de usuario.
 * Al ser un servicio "root" (único en toda la app) NO se reinicia solo al cerrar
 * sesión, por eso es necesario limpiarlo explícitamente.
 */
@Injectable({ providedIn: 'root' })
export class CarritoService {
  readonly items = signal<ItemCarrito[]>([]);

  /** Total de unidades en el carrito. */
  readonly totalProductos = computed(() =>
    this.items().reduce((total, item) => total + item.cantidad, 0),
  );

  /** Reinicia el carrito a su estado inicial (vacío). */
  limpiar(): void {
    this.items.set([]);
  }
}