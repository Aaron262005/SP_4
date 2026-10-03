import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map, switchMap, tap, throwError } from 'rxjs';
import { API_URL } from '../../core/api.config';
import { LoginRequest } from '../../models/auth/login-request.model';
import { LoginResponse } from '../../models/auth/login-response.model';
import { Sesion } from '../../models/auth/sesion.model';
import { Usuario } from '../../models/auth/usuario.model';
import { CarritoService } from '../carrito/carrito.service';
import { RolAsignadorService } from './rol-asignador.service';
import { SessionService } from './session.service';
import { TokenDecoderService } from './token-decoder.service';

/**
 * Casos de uso de autenticación del frontend: iniciar y cerrar sesión.
 * Coordina a los demás servicios; cada uno hace una sola tarea.
 */
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly tokenDecoder = inject(TokenDecoderService);
  private readonly rolAsignador = inject(RolAsignadorService);
  private readonly session = inject(SessionService);
  private readonly carrito = inject(CarritoService);

  /**
   * US01 - Inicio de sesión. Pasos:
   *  1. POST /auth/login  -> recibe el token.
   *  2. Decodifica el token para obtener el ID del usuario.
   *  3. Asigna el rol localmente según ese ID.
   *  4. GET /usuarios/{id} -> descarga la información del usuario.
   *  5. Guarda la sesión.
   */
  iniciarSesion(credenciales: LoginRequest): Observable<Sesion> {
    return this.http.post<LoginResponse>(`${API_URL}/auth/login`, credenciales).pipe(
      switchMap(({ token }) => {
        const usuarioId = this.tokenDecoder.obtenerIdUsuario(token);
        if (usuarioId === null) {
          return throwError(() => new Error('El token recibido no es válido.'));
        }

        const rol = this.rolAsignador.asignarRol(usuarioId);

        return this.http
          .get<Usuario>(`${API_URL}/usuarios/${usuarioId}`)
          .pipe(map((usuario): Sesion => ({ token, usuarioId, rol, nombre: usuario.nombre })));
      }),
      // Solo si todo salió bien se guarda la sesión.
      tap((sesion) => this.session.guardar(sesion)),
    );
  }

  /**
   * US02 - Cierre de sesión: borra la sesión (token, ID, rol) y reinicia el carrito.
   * La redirección al Login la hace el ViewModel.
   */
  cerrarSesion(): void {
    this.session.limpiar();
    this.carrito.limpiar();
  }
}