import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DirectorioUsuario } from '../../models/directorio/directorio.model';
import { API_URL } from '../../core/api.config'; // Ajusta la ruta a tu api.config.ts

@Injectable({ providedIn: 'root' })
export class DirectorioService {
  private readonly http = inject(HttpClient);

  obtenerDirectorio(): Observable<DirectorioUsuario[]> {
    return this.http.get<DirectorioUsuario[]>(`${API_URL}/api/directorio`);
  }
}