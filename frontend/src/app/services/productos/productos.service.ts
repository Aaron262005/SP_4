import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL } from '../../core/api.config';
import { Producto } from '../../models/productos/producto.model';

/**
 * SERVICIO de productos (capa Model): su única responsabilidad es
 * comunicarse con la API. No guarda estado ni conoce la pantalla.
 */
@Injectable({ providedIn: 'root' })
export class ProductosService {
  private readonly http = inject(HttpClient);

  /** Pide a la API la lista completa de productos (GET /products). */
  listar(): Observable<Producto[]> {
    return this.http.get<Producto[]>(`${API_URL}/products`);
  }
}