import { Injectable, signal, inject } from '@angular/core';
import { HistorialCarritosService } from '../../services/historial-carritos/historial-carritos.service';
import { HistorialCarrito } from '../../models/historial-carritos/historial-carritos.model';

@Injectable()
export class HistorialCarritosViewModel {
  private readonly historialService = inject(HistorialCarritosService);

  readonly carritos = signal<HistorialCarrito[]>([]);
  readonly cargando = signal(false);
  readonly error = signal<string | null>(null);
  readonly carritoExpandidoId = signal<number | null>(null); // Controla qué carrito está abierto

  cargarCarritos(): void {
    this.cargando.set(true);
    this.error.set(null);
    
    this.historialService.obtenerHistorial().subscribe({
      next: (data) => { 
        this.carritos.set(data); 
        this.cargando.set(false); 
      },
      error: () => { 
        this.error.set('No se pudo cargar el historial de carritos.'); 
        this.cargando.set(false); 
      }
    });
  }

  toggleExpandir(id: number): void {
    // Si hace clic en el que ya está abierto, lo cierra. Si no, lo abre.
    this.carritoExpandidoId.set(this.carritoExpandidoId() === id ? null : id);
  }
}