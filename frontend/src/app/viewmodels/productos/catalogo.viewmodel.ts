import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';
import { Producto } from '../../models/productos/producto.model';
import { ProductosService } from '../../services/productos/productos.service';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { mensajeErrorProductos } from './error-productos';

/** Estado del catálogo mínimo necesario para navegar por US06, US07 y US08. */
@Injectable()
export class CatalogoViewModel {
  private readonly productosService = inject(ProductosService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly avisos = inject(AvisoProductosService);
  readonly esAdministrador = inject(AccesoProductosService).esAdministrador;
  readonly productos = signal<Producto[]>([]);
  readonly cargando = signal(false);
  readonly error = signal('');
  readonly mensaje = this.avisos.mensaje;
  cerrarAviso(): void { this.avisos.cerrar(); }
  cargar(): void {
    if (this.cargando()) return;
    this.cargando.set(true); this.error.set('');
    this.productosService.listar().pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.cargando.set(false))).subscribe({
      next: productos => this.productos.set(productos),
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
}
