import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { CatalogoViewModel } from '../../viewmodels/productos/catalogo.viewmodel';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

/** View standalone: presenta el estado y delega las acciones al ViewModel. */
@Component({
  selector: 'app-catalogo', standalone: true,
  imports: [CommonModule, RouterLink, AlertaComponent],
  providers: [CatalogoViewModel],
  templateUrl: './catalogo.view.html', styleUrl: './catalogo.view.scss',
})
export class CatalogoView implements OnInit {
  readonly vm = inject(CatalogoViewModel);
  ngOnInit(): void { this.vm.cargar(); }
}
