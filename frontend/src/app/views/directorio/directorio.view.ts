import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DirectorioViewModel } from '../../viewmodels/directorio/directorio.viewmodel';
// IMPORTANTE: Asegúrate de que la ruta a AlertaComponent sea correcta según tu proyecto
import { AlertaComponent } from '../../shared/alerta/alerta.component'; 

@Component({
  selector: 'app-directorio',
  standalone: true,
  imports: [CommonModule, AlertaComponent],
  providers: [DirectorioViewModel],
  templateUrl: './directorio.view.html',
  styleUrl: './directorio.view.scss'
})
export class DirectorioView implements OnInit {
  readonly vm = inject(DirectorioViewModel);

  ngOnInit(): void {
    this.vm.cargarUsuarios();
  }
}