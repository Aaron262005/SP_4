import { Injectable, inject, signal } from '@angular/core';
import { Producto } from '../../models/productos/producto.model';
import { ProductosService } from '../../services/productos/productos.service';

/**
 * VIEWMODEL del catálogo: guarda el estado de la pantalla con signals
 * y expone las acciones. No conoce el HTML ni el DOM.
 */
@Injectable()
export class CatalogoViewModel {
  private readonly servicio = inject(ProductosService);

  /** Lista de productos que se muestra en la cuadrícula. */
  readonly productos = signal<Producto[]>([]);
  /** Es true mientras se espera la respuesta (muestra el spinner). */
  readonly cargando = signal(false);
  /** Mensaje amigable si la carga falla; null cuando todo va bien. */
  readonly error = signal<string | null>(null);

  /** Descarga el catálogo y actualiza los tres estados de la pantalla. */
  cargar(): void {
    this.cargando.set(true);
    this.error.set(null);

    this.servicio.listar().subscribe({
      next: (lista) => {
        this.productos.set(lista);
        this.cargando.set(false);
      },
      error: () => {
        // Se detiene el spinner y se muestra un mensaje sin tecnicismos.
        this.productos.set([]);
        this.error.set('No pudimos cargar el catálogo. Revisa tu conexión e inténtalo de nuevo.');
        this.cargando.set(false);
      },
    });
  }

  /** Acción del botón "Reintentar": repite la petición. */
  reintentar(): void {
    this.cargar();
  }
}