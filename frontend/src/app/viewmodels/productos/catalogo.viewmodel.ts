import { Injectable, inject, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { Producto } from '../../models/productos/producto.model';
import { CategoriasService } from '../../services/productos/categorias.service';
import { ProductosService } from '../../services/productos/productos.service';

/**
 * VIEWMODEL del catálogo: guarda el estado de la pantalla con signals
 * y expone las acciones. No conoce el HTML ni el DOM.
 * US03: carga del catálogo. US04: filtro por categoría.
 */
@Injectable()
export class CatalogoViewModel {
  private readonly servicio = inject(ProductosService);
  private readonly categoriasServicio = inject(CategoriasService);

  /** Lista de productos que se muestra en la cuadrícula. */
  readonly productos = signal<Producto[]>([]);
  /** Es true mientras se espera la respuesta (muestra el spinner). */
  readonly cargando = signal(false);
  /** Mensaje amigable si la carga falla; null cuando todo va bien. */
  readonly error = signal<string | null>(null);

  // ===== US04 =====
  /** Categorías disponibles para los chips de filtro. */
  readonly categorias = signal<string[]>([]);
  /** Categoría seleccionada; null significa "Ver todos". */
  readonly categoriaActiva = signal<string | null>(null);
  // ===== FIN US04 =====

  /** Número de la última petición lanzada: sirve para ignorar respuestas atrasadas. */
  private peticionActual = 0;

  /** Descarga el catálogo completo (GET /products). */
  cargar(): void {
    this.ejecutarCarga(this.servicio.listar());
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
   * Lanza una petición de productos y actualiza los tres estados de la pantalla.
   * Antes de pedir se vacía la lista para no mostrar datos de la vista anterior.
   */
  private ejecutarCarga(peticion: Observable<Producto[]>): void {
    const numero = ++this.peticionActual;
    this.productos.set([]);
    this.cargando.set(true);
    this.error.set(null);

    peticion.subscribe({
      next: (lista) => {
        if (numero !== this.peticionActual) return; // llegó tarde: ya hay otra petición
        this.productos.set(lista);
        this.cargando.set(false);
      },
      error: () => {
        if (numero !== this.peticionActual) return;
        // Se detiene el spinner y se muestra un mensaje sin tecnicismos.
        this.productos.set([]);
        this.error.set('No pudimos cargar el catálogo. Revisa tu conexión e inténtalo de nuevo.');
        this.cargando.set(false);
      },
    });
  }
}