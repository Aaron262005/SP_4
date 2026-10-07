import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { CatalogoViewModel } from '../../viewmodels/productos/catalogo.viewmodel';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

/**
 * VIEW del catálogo: solo presenta. Todo el estado y las acciones
 * vienen del ViewModel; esta clase nunca llama a un servicio.
 * Standalone: declara aquí lo que usa su plantilla.
 */
@Component({
  selector: 'app-catalogo',
  standalone: true,
  // CommonModule aporta el pipe "number" y el control de flujo; RouterLink, los enlaces;
  // AlertaComponent, el cuadro de error compartido (<app-alerta>).
  imports: [CommonModule, RouterLink, AlertaComponent],
  // El ViewModel se crea junto con la pantalla y se destruye con ella.
  providers: [CatalogoViewModel],
  templateUrl: './catalogo.view.html',
  styleUrl: './catalogo.view.scss',
})
export class CatalogoView implements OnInit {
  readonly vm = inject(CatalogoViewModel);

  /** Al entrar a la pantalla se pide el catálogo. */
  ngOnInit(): void {
    this.vm.cargar();
  }
}