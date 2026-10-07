import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormularioProductoViewModel } from '../../viewmodels/productos/formulario-producto.viewmodel';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

/** View standalone: presenta el estado y delega las acciones al ViewModel. */
@Component({
  selector: 'app-formulario-producto', standalone: true,
  imports: [CommonModule, RouterLink, AlertaComponent, ReactiveFormsModule],
  providers: [FormularioProductoViewModel],
  templateUrl: './formulario-producto.view.html', styleUrl: './formulario-producto.view.scss',
})
export class FormularioProductoView implements OnInit {
  readonly vm = inject(FormularioProductoViewModel);
  ngOnInit(): void { this.vm.cargar(); }
}
