import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, defer, throwError } from 'rxjs';
import { API_URL } from '../../core/api.config';
import { Producto, GuardarProducto } from '../../models/productos/producto.model';
import { SessionService } from '../auth/session.service';
import { AccesoProductosService } from './acceso-productos.service';

/**
 * SERVICIO de productos (capa Model): su única responsabilidad es comunicarse
 * con la API y bloquear antes las escrituras de quien no tiene permisos.
 * No guarda estado ni administra la pantalla.
 */
@Injectable({ providedIn: 'root' })
export class ProductosService {
  private readonly http = inject(HttpClient);
  private readonly sesion = inject(SessionService);
  private readonly acceso = inject(AccesoProductosService);

  /** US03: pide a la API la lista completa de productos (GET /products). */
  listar(): Observable<Producto[]> {
    return this.http.get<Producto[]>(`${API_URL}/products`);
  }

  /** Pide el detalle de un producto (GET /products/{id}). */
  obtener(id: number): Observable<Producto> {
    return this.http.get<Producto>(`${API_URL}/products/${id}`);
  }

  /** US06: crea un producto (POST /products). Solo administradores. */
  agregar(datos: GuardarProducto): Observable<Producto> {
    return this.autorizar(() => this.http.post<Producto>(`${API_URL}/products`, datos, this.opciones()));
  }

  /** US07: actualiza un producto (PUT /products/{id}). Solo administradores. */
  editar(id: number, datos: GuardarProducto): Observable<Producto> {
    return this.autorizar(() => this.http.put<Producto>(`${API_URL}/products/${id}`, datos, this.opciones()));
  }

  /** US08: elimina un producto (DELETE /products/{id}). Solo administradores. */
  eliminar(id: number): Observable<Producto> {
    return this.autorizar(() => this.http.delete<Producto>(`${API_URL}/products/${id}`, this.opciones()));
  }

  /** Cabecera con el token de la sesión, que la API valida en las escrituras. */
  private opciones() {
    return { headers: { Authorization: `Bearer ${this.sesion.token()}` } };
  }

  /**
   * Comprueba los permisos al suscribirse, antes de construir una petición de red.
   * Si el usuario no puede modificar, devuelve un error sin llamar a la API.
   */
  private autorizar(accion: () => Observable<Producto>): Observable<Producto> {
    return defer(() =>
      this.acceso.puedeModificar()
        ? accion()
        : throwError(() => new Error('Solo un Administrador con sesión vigente puede modificar productos.')),
    );
  }
}