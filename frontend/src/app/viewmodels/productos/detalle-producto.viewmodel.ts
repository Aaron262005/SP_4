import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { Producto } from '../../models/productos/producto.model';
import { ProductosService } from '../../services/productos/productos.service';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { mensajeErrorProductos } from './error-productos';

/** Detalle y US08. Recibe la decisión del diálogo sin conocer el navegador ni el DOM. */
@Injectable()
export class DetalleProductoViewModel {
  private readonly servicio = inject(ProductosService);
  private readonly acceso = inject(AccesoProductosService);
  private readonly avisos = inject(AvisoProductosService);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);
  readonly id = Number(inject(ActivatedRoute).snapshot.paramMap.get('id'));
  readonly esAdministrador = this.acceso.esAdministrador;
  readonly producto = signal<Producto | null>(null);
  readonly cargando = signal(false);
  readonly eliminando = signal(false);
  readonly error = signal('');
  readonly mensaje = this.avisos.mensaje;
  cerrarAviso(): void { this.avisos.cerrar(); }
  cargar(): void {
    if (this.cargando()) return;
    this.cargando.set(true); this.error.set('');
    this.servicio.obtener(this.id).pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.cargando.set(false))).subscribe({
      next: producto => this.producto.set(producto),
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
  eliminar(confirmado: boolean): void {
    if (!confirmado || this.eliminando() || !this.producto()) return;
    if (!this.acceso.puedeModificar()) { this.error.set('No tienes permisos para eliminar.'); return; }
    this.eliminando.set(true); this.error.set('');
    this.servicio.eliminar(this.id).pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.eliminando.set(false))).subscribe({
      next: () => {
        this.avisos.mostrar('Producto eliminado correctamente (Simulación).');
        void this.router.navigate(['/productos']);
      },
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
}
