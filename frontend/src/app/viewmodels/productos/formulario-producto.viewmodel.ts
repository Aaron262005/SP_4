import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { ProductosService } from '../../services/productos/productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';
import { mensajeErrorProductos } from './error-productos';

/** Validaciones locales compartidas: nunca se envían formularios inválidos. */
const textoObligatorio: ValidatorFn = control => typeof control.value === 'string' && control.value.trim() ? null : { required: true };
const numeroValido: ValidatorFn = control => typeof control.value === 'number' && Number.isFinite(control.value) ? null : { numero: true };
const urlValida: ValidatorFn = control => {
  try { const url = new URL(control.value); return ['http:', 'https:'].includes(url.protocol) ? null : { url: true }; }
  catch { return { url: true }; }
};

/** US06 y US07 comparten formulario; el ViewModel coordina validación, carga y guardado. */
@Injectable()
export class FormularioProductoViewModel {
  private readonly servicio = inject(ProductosService);
  private readonly acceso = inject(AccesoProductosService);
  private readonly avisos = inject(AvisoProductosService);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);
  readonly id = Number(inject(ActivatedRoute).snapshot.paramMap.get('id')) || null;
  readonly cargando = signal(false);
  readonly guardando = signal(false);
  readonly listo = signal(this.id === null);
  readonly error = signal('');
  readonly mensaje = signal('');
  readonly formulario = new FormGroup({
    titulo: new FormControl('', { nonNullable: true, validators: [textoObligatorio] }),
    precio: new FormControl<number | null>(null, [Validators.required, numeroValido]),
    descripcion: new FormControl('', { nonNullable: true, validators: [textoObligatorio] }),
    categoria: new FormControl('', { nonNullable: true, validators: [textoObligatorio] }),
    imagen: new FormControl('', { nonNullable: true, validators: [textoObligatorio, urlValida] }),
  });
  cargar(): void {
    this.avisos.cerrar();
    if (this.id === null || this.cargando()) return;
    this.cargando.set(true); this.listo.set(false); this.error.set('');
    this.servicio.obtener(this.id).pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.cargando.set(false))).subscribe({
      next: producto => { this.formulario.patchValue(producto); this.listo.set(true); },
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
  invalido(campo: keyof typeof this.formulario.controls): boolean {
    const control = this.formulario.controls[campo];
    return control.invalid && control.touched;
  }
  guardar(): void {
    if (this.guardando() || !this.listo()) return;
    this.error.set(''); this.mensaje.set('');
    if (!this.acceso.puedeModificar()) { void this.router.navigate(['/productos']); return; }
    this.formulario.markAllAsTouched();
    if (this.formulario.invalid) { this.error.set('Corrige los campos marcados antes de guardar.'); return; }
    const valores = this.formulario.getRawValue();
    const datos = { ...valores, precio: valores.precio!, titulo: valores.titulo.trim(), descripcion: valores.descripcion.trim(), categoria: valores.categoria.trim(), imagen: valores.imagen.trim() };
    this.guardando.set(true);
    const peticion = this.servicio.agregar(datos);
    peticion.pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.guardando.set(false))).subscribe({
      next: producto => {
        if (this.id === null) {
          this.mensaje.set(`Producto creado (Simulación). ID generado: ${producto.id}.`);
          this.formulario.reset();
        } else {
          this.avisos.mostrar('Producto actualizado (Simulación)');
          void this.router.navigate(['/productos', producto.id]);
        }
      },
      error: error => this.error.set(mensajeErrorProductos(error)),
    });
  }
}
