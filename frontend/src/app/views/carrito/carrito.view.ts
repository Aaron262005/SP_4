import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarritoViewModel } from '../../viewmodels/carrito/carrito.viewmodel';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

@Component({
  selector: 'app-carrito-view',
  standalone: true,
  imports: [CommonModule, AlertaComponent],
  providers: [CarritoViewModel],
  templateUrl: './carrito.view.html',
  styleUrl: './carrito.view.scss'
})
export class CarritoView {
  readonly vm = inject(CarritoViewModel);
}