import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';
import { Producto } from '../../models/productos/producto.model';
import { ProductosService } from '../../services/productos/productos.service';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { mensajeErrorProductos } from './error-productos';

/**
 * VIEWMODEL del catálogo: guarda el estado de la pantalla con signals
 * y expone las acciones. No conoce el HTML ni el DOM.
 * Lo comparten US03 (catálogo) y US06, US07 y US08 (agregar, editar y eliminar).
 */
@Injectable()
export class CatalogoViewModel {
  private readonly productosService = inject(ProductosService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly avisos = inject(AvisoProductosService);

  /** Indica si el usuario puede agregar, editar y eliminar (muestra u oculta esos botones). */
  readonly esAdministrador = inject(AccesoProductosService).esAdministrador;

  /** Lista de productos que se muestra en la cuadrícula. */
  readonly productos = signal<Producto[]>([]);
  /** Es true mientras se espera la respuesta (muestra el spinner). */
  readonly cargando = signal(false);
  /** Mensaje amigable si la carga falla; cadena vacía cuando todo va bien. */
  readonly error = signal('');
  /** Aviso de resultado de otras pantallas (por ejemplo, "producto agregado"). */
  readonly mensaje = this.avisos.mensaje;

  /** Cierra el aviso de resultado. */
  cerrarAviso(): void {
    this.avisos.cerrar();
  }

  /** Descarga el catálogo y actualiza los estados de la pantalla. */
  cargar(): void {
    // Evita lanzar una segunda petición mientras la primera sigue en curso.
    if (this.cargando()) return;

    this.cargando.set(true);
    this.error.set('');

    this.productosService
      .listar()
      .pipe(
        // Cancela la petición si la pantalla se destruye antes de recibir la respuesta.
        takeUntilDestroyed(this.destroyRef),
        // Apaga el spinner siempre, tanto si la carga termina bien como si falla.
        finalize(() => this.cargando.set(false)),
      )
      .subscribe({
        next: (productos) => this.productos.set(productos),
        // Se convierte el error técnico en un mensaje sin tecnicismos.
        error: (error) => this.error.set(mensajeErrorProductos(error)),
      });
  }

  /** US03: acción del botón "Reintentar"; repite la petición del catálogo. */
  reintentar(): void {
    this.cargar();
  }
}