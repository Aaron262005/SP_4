import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, defer, throwError } from 'rxjs';
import { API_URL } from '../../core/api.config';
import { Producto, GuardarProducto } from '../../models/productos/producto.model';
import { SessionService } from '../auth/session.service';
import { AccesoProductosService } from './acceso-productos.service';

/** Model: comunicación HTTP y bloqueo previo de escrituras sin permisos. No administra la pantalla. */
@Injectable({ providedIn: 'root' })
export class ProductosService {
  private readonly http = inject(HttpClient);
  private readonly sesion = inject(SessionService);
  private readonly acceso = inject(AccesoProductosService);
  listar(): Observable<Producto[]> { return this.http.get<Producto[]>(`${API_URL}/products`); }
  obtener(id: number): Observable<Producto> { return this.http.get<Producto>(`${API_URL}/products/${id}`); }
  agregar(datos: GuardarProducto): Observable<Producto> {
    return this.autorizar(() => this.http.post<Producto>(`${API_URL}/products`, datos, this.opciones()));
  }
  editar(id: number, datos: GuardarProducto): Observable<Producto> {
    return this.autorizar(() => this.http.put<Producto>(`${API_URL}/products/${id}`, datos, this.opciones()));
  }
  private opciones() { return { headers: { Authorization: `Bearer ${this.sesion.token()}` } }; }
  private autorizar(accion: () => Observable<Producto>): Observable<Producto> {
    // Se comprueban los permisos al suscribirse, antes de construir una petición de red.
    return defer(() => this.acceso.puedeModificar() ? accion() : throwError(() => new Error('Solo un Administrador con sesión vigente puede modificar productos.')));
  }
}
