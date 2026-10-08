import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { API_URL } from '../../core/api.config';
import { ItemCarrito, AgregarItemRequest } from '../../models/carrito/item-carrito.model';

@Injectable({
  providedIn: 'root'
})
export class CarritoService {
  private readonly http = inject(HttpClient);

  private readonly itemsSignal = signal<ItemCarrito[]>([]);
  readonly items = this.itemsSignal.asReadonly();

  // Signal calculada con el total acumulado de la compra (US10)
  readonly totalMonto = computed(() =>
    this.itemsSignal().reduce((acc, item) => acc + (item.precioUnitario * item.cantidad), 0)
  );

  readonly totalProductos = computed(() =>
    this.itemsSignal().reduce((acc, item) => acc + item.cantidad, 0)
  );

  // ===== US10: Cargar Carrito del Usuario =====
  obtenerCarrito(usuarioId: number): Observable<ItemCarrito[]> {
    return this.http.get<ItemCarrito[]>(`${API_URL}/carrito/usuario/${usuarioId}`).pipe(
      tap(items => this.itemsSignal.set(items))
    );
  }

  // ===== US09: Agregar Ítem =====
  agregarItem(request: AgregarItemRequest): Observable<ItemCarrito> {
    return this.http.post<ItemCarrito>(`${API_URL}/carrito`, request).pipe(
      tap((itemProcesado) => {
        this.itemsSignal.update((lista) => {
          const index = lista.findIndex((item) => item.productoId === itemProcesado.productoId);
          if (index !== -1) {
            const copia = [...lista];
            copia[index] = itemProcesado;
            return copia;
          }
          return [...lista, itemProcesado];
        });
      })
    );
  }

  // ===== US10: Modificar Cantidad =====
  actualizarCantidad(itemId: number, nuevaCantidad: number): Observable<ItemCarrito> {
    return this.http.put<ItemCarrito>(`${API_URL}/carrito/${itemId}/cantidad`, nuevaCantidad).pipe(
      tap((itemActualizado) => {
        this.itemsSignal.update(lista =>
          lista.map(i => i.id === itemId ? itemActualizado : i)
        );
      })
    );
  }

  // ===== US10: Eliminar Ítem =====
  eliminarItem(itemId: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/carrito/${itemId}`).pipe(
      tap(() => {
        this.itemsSignal.update(lista => lista.filter(i => i.id !== itemId));
      })
    );
  }

  limpiar(): void {
    this.itemsSignal.set([]);
  }
}