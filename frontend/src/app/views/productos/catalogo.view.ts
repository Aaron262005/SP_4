import { CurrencyPipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CatalogoViewModel } from '../../viewmodels/productos/catalogo.viewmodel';

/**
 * VIEW del catálogo: solo presenta. Todo el estado y las acciones
 * vienen del ViewModel; esta clase nunca llama a un servicio.
 */
@Component({
  selector: 'app-catalogo',
  imports: [CurrencyPipe, RouterLink],
  providers: [CatalogoViewModel],
  templateUrl: './catalogo.view.html',
  styleUrl: './catalogo.view.scss',
})
export class CatalogoView implements OnInit {
  readonly vm = inject(CatalogoViewModel);

  /** Al entrar a la pantalla se piden las categorías (US04) y el catálogo (US03). */
  ngOnInit(): void {
    this.vm.cargarCategorias();
    this.vm.cargar();
  }
}