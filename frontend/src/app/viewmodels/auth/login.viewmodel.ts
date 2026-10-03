import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ConectividadService } from '../../core/conectividad.service';
import { AuthService } from '../../services/auth/auth.service';

/**
 * VIEWMODEL de la pantalla de Login.
 * Guarda el estado de la pantalla (campos, errores, "cargando") y la acción de iniciar sesión.
 * No conoce el HTML: la vista solo se enlaza a estas propiedades.
 */
@Injectable()
export class LoginViewModel {
  private readonly auth = inject(AuthService);
  private readonly conectividad = inject(ConectividadService);
  private readonly router = inject(Router);

  // --- Estado de la pantalla (signals) ---
  readonly nombreUsuario = signal('');
  readonly contrasena = signal('');
  readonly mostrarContrasena = signal(false);
  readonly cargando = signal(false);
  readonly error = signal<string | null>(null);

  /** El botón solo se habilita cuando ambos campos tienen contenido. */
  readonly formularioValido = computed(
    () => this.nombreUsuario().trim().length > 0 && this.contrasena().length > 0,
  );

  /** Muestra u oculta la contraseña. */
  alternarVisibilidadContrasena(): void {
    this.mostrarContrasena.update((visible) => !visible);
  }

  /** Acción del botón "Iniciar sesión". */
  iniciarSesion(): void {
    // Evita enviar dos veces o con campos vacíos.
    if (this.cargando() || !this.formularioValido()) return;

    this.error.set(null);

    // US01 - Escenario 3: sin internet, NO se llama a la API y se avisa al usuario.
    if (!this.conectividad.hayConexion()) {
      this.error.set('Sin conexión a internet. Revisa tu red e inténtalo de nuevo.');
      return;
    }

    this.cargando.set(true);

    this.auth
      .iniciarSesion({
        nombreUsuario: this.nombreUsuario().trim(),
        contrasena: this.contrasena(),
      })
      .subscribe({
        // US01 - Escenario 1: éxito. replaceUrl evita que "Atrás" regrese al Login.
        next: () => {
          this.cargando.set(false);
          this.router.navigate(['/inicio'], { replaceUrl: true });
        },
        // US01 - Escenario 2: error (por ejemplo 401) => alerta roja.
        error: (error: unknown) => {
          this.cargando.set(false);
          this.error.set(this.obtenerMensajeDeError(error));
        },
      });
  }

  /** Traduce el error técnico a un mensaje claro para el usuario. */
  private obtenerMensajeDeError(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 401) return 'Usuario o contraseña inválidos';
      if (error.status === 400) return 'Revisa que el usuario y la contraseña estén completos.';
      if (error.status === 0) return 'No se pudo conectar con el servidor. Inténtalo más tarde.';
    }
    return 'Ocurrió un error inesperado. Inténtalo de nuevo.';
  }
}