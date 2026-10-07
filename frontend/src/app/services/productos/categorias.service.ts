import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL } from '../../core/api.config';
import { Producto } from '../../models/productos/producto.model';

/**
 * SERVICIO de categorías (capa Model): solo se comunica con la API.
 * Es un servicio aparte de ProductosService para no modificar el de la US03.
 */
@Injectable({ providedIn: 'root' })
export class CategoriasService {
  private readonly http = inject(HttpClient);

  /** Pide los nombres de las categorías (GET /products/categories). */
  listar(): Observable<string[]> {
    return this.http.get<string[]>(`${API_URL}/products/categories`);
  }

  /** Pide solo los productos de una categoría (GET /products/category/{categoria}). */
  productosDe(categoria: string): Observable<Producto[]> {
    // encodeURIComponent protege los espacios y acentos, por ejemplo "Ropa de hombre".
    return this.http.get<Producto[]>(`${API_URL}/products/category/${encodeURIComponent(categoria)}`);
  }
}