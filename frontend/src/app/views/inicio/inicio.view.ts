import { Component, inject } from '@angular/core';
import { InicioViewModel } from '../../viewmodels/inicio/inicio.viewmodel';

/**
 * VIEW de la pantalla principal (a la que se llega después de iniciar sesión).
 * Muestra una interfaz distinta según el rol del usuario.
 */
@Component({
  selector: 'app-inicio',
  providers: [InicioViewModel],
  templateUrl: './inicio.view.html',
  styleUrl: './inicio.view.scss',
})
export class InicioView {
  readonly vm = inject(InicioViewModel);
}