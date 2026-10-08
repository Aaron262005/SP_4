import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable, finalize } from 'rxjs';
import { Producto } from '../../models/productos/producto.model';
import { CategoriasService } from '../../services/productos/categorias.service';
import { ProductosService } from '../../services/productos/productos.service';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { mensajeErrorProductos } from './error-productos';

/**
 * VIEWMODEL del catálogo: guarda el estado de la pantalla con signals
 * y expone las acciones. No conoce el HTML ni el DOM.
 * Lo comparten US03 (catálogo), US04 (filtro por categoría) y US06, US07 y US08 (agregar, editar y eliminar).
 */
@Injectable()
export class CatalogoViewModel {
  private readonly productosService = inject(ProductosService);
  private readonly categoriasServicio = inject(CategoriasService);
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

  // ===== US04 =====
  /** Categorías disponibles para los chips de filtro. */
  readonly categorias = signal<string[]>([]);
  /** Categoría seleccionada; null significa "Ver todos". */
  readonly categoriaActiva = signal<string | null>(null);
  // ===== FIN US04 =====

  /** Número de la última petición lanzada: sirve para ignorar respuestas atrasadas. */
  private peticionActual = 0;

  /** Cierra el aviso de resultado. */
  cerrarAviso(): void {
    this.avisos.cerrar();
  }

  /** Descarga el catálogo completo (GET /products). */
  cargar(): void {
    this.ejecutarCarga(this.productosService.listar());
  }

  /** Descarga las categorías para los chips (GET /products/categories). */
  cargarCategorias(): void {
    this.categoriasServicio.listar().subscribe({
      next: (lista) => this.categorias.set(lista),
      // Si fallan las categorías, simplemente no se muestran los chips.
      error: () => this.categorias.set([]),
    });
  }

  /** Al tocar un chip: filtra por esa categoría; si ya estaba activa, quita el filtro. */
  seleccionarCategoria(categoria: string): void {
    if (this.categoriaActiva() === categoria) {
      this.verTodos();
      return;
    }
    this.categoriaActiva.set(categoria);
    this.ejecutarCarga(this.categoriasServicio.productosDe(categoria));
  }

  /** Quita el filtro y vuelve a pedir el catálogo completo. */
  verTodos(): void {
    this.categoriaActiva.set(null);
    this.cargar();
  }

  /** Acción del botón "Reintentar": repite la petición según el filtro actual. */
  reintentar(): void {
    const categoria = this.categoriaActiva();
    if (categoria) {
      this.ejecutarCarga(this.categoriasServicio.productosDe(categoria));
    } else {
      this.cargar();
    }
  }

  /**
   * Lanza una petición de productos y actualiza los estados de la pantalla.
   * Se cancela si la pantalla se destruye antes de recibir la respuesta,
   * y se descarta si llega después de que ya se pidió otra cosa (cambio rápido de chip).
   */
  private ejecutarCarga(peticion: Observable<Producto[]>): void {
    const numero = ++this.peticionActual;
    this.productos.set([]);
    this.cargando.set(true);
    this.error.set('');

    peticion
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          if (numero === this.peticionActual) this.cargando.set(false);
        }),
      )
      .subscribe({
        next: (lista) => {
          if (numero !== this.peticionActual) return; // llegó tarde: ya hay otra petición
          this.productos.set(lista);
        },
        error: (error: unknown) => {
          if (numero !== this.peticionActual) return;
          this.productos.set([]);
          this.error.set(mensajeErrorProductos(error));
        },
      });
  }
}
