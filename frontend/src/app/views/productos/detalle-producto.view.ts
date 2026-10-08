import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { DetalleProductoViewModel } from '../../viewmodels/productos/detalle-producto.viewmodel';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

/** View standalone: presenta el estado y delega las acciones al ViewModel. */
@Component({
  selector: 'app-detalle-producto', standalone: true,
  imports: [CommonModule, RouterLink, AlertaComponent],
  providers: [DetalleProductoViewModel],
  templateUrl: './detalle-producto.view.html', styleUrl: './detalle-producto.view.scss',
})
export class DetalleProductoView implements OnInit {
  readonly vm = inject(DetalleProductoViewModel);
  ngOnInit(): void { this.vm.cargar(); }
  confirmarEliminacion(): void {
    // El diálogo nativo pertenece a la View; cancelar no invoca ninguna petición.
    if (!this.vm.eliminando()) this.vm.eliminar(window.confirm('¿Estás seguro de eliminar este producto?'));
  }
}
