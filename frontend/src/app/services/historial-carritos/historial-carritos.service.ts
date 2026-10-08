import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HistorialCarrito } from '../../models/historial-carritos/historial-carritos.model';
import { API_URL } from '../../core/api.config';

@Injectable({ providedIn: 'root' })
export class HistorialCarritosService {
  private readonly http = inject(HttpClient);
  
  obtenerHistorial(): Observable<HistorialCarrito[]> {
    return this.http.get<HistorialCarrito[]>(`${API_URL}/api/historialcarritos`);
  }
}