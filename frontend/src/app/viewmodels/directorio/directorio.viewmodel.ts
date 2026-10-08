import { Injectable, signal, inject } from '@angular/core';
import { DirectorioService } from '../../services/directorio/directorio.service';
import { DirectorioUsuario } from '../../models/directorio/directorio.model';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class DirectorioViewModel {
  private readonly directorioService = inject(DirectorioService);

  // Signals para manejar el estado
  readonly usuarios = signal<DirectorioUsuario[]>([]);
  readonly cargando = signal<boolean>(false);
  readonly error = signal<string | null>(null);

  cargarUsuarios(): void {
    this.cargando.set(true);
    this.error.set(null);

    this.directorioService.obtenerDirectorio().subscribe({
      next: (data) => {
        this.usuarios.set(data);
        this.cargando.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.error.set('Error de conexión al cargar el directorio. Intente de nuevo.');
        this.cargando.set(false);
      }
    });
  }
}