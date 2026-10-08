import { Injectable, inject, signal, computed, OnInit } from '@angular/core';
import { CarritoService } from '../../services/carrito/carrito.service';
import { SessionService } from '../../services/auth/session.service';

@Injectable()
export class CarritoViewModel implements OnInit {
  private readonly carritoService = inject(CarritoService);
  private readonly sessionService = inject(SessionService);

  readonly cargando = signal<boolean>(false);
  readonly error = signal<string | null>(null);

  // Expone las signals del servicio a la vista
  readonly items = this.carritoService.items;
  readonly totalMonto = this.carritoService.totalMonto;

  // US10 - Restricción: El Auditor solo visualiza, no puede editar ni eliminar
  readonly esAuditor = computed(() => this.sessionService.rol() === 'Auditor');

  ngOnInit(): void {
    this.cargarCarrito();
  }

  cargarCarrito(): void {
    const usuarioId = this.sessionService.usuarioId();
    if (!usuarioId) return;

    this.cargando.set(true);
    this.carritoService.obtenerCarrito(usuarioId).subscribe({
      next: () => this.cargando.set(false),
      error: () => {
        this.cargando.set(false);
        this.error.set('Error al cargar los artículos del carrito.');
      }
    });
  }

  aumentarCantidad(itemId: number, cantidadActual: number): void {
    if (this.esAuditor()) return;
    this.actualizarCantidad(itemId, cantidadActual + 1);
  }

  disminuirCantidad(itemId: number, cantidadActual: number): void {
    if (this.esAuditor()) return;
    if (cantidadActual <= 1) {
      this.eliminarItem(itemId);
    } else {
      this.actualizarCantidad(itemId, cantidadActual - 1);
    }
  }

  private actualizarCantidad(itemId: number, nuevaCantidad: number): void {
    this.carritoService.actualizarCantidad(itemId, nuevaCantidad).subscribe({
      error: () => this.error.set('No se pudo actualizar la cantidad.')
    });
  }

  eliminarItem(itemId: number): void {
    if (this.esAuditor()) return;
    this.carritoService.eliminarItem(itemId).subscribe({
      error: () => this.error.set('No se pudo eliminar el artículo.')
    });
  }
}