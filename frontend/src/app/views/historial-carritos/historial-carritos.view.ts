import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HistorialCarritosViewModel } from '../../viewmodels/historial-carritos/historial-carritos.viewmodel';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

@Component({
  selector: 'app-historial-carritos',
  standalone: true,
  imports: [CommonModule, AlertaComponent],
  providers: [HistorialCarritosViewModel],
  templateUrl: './historial-carritos.view.html',
  styleUrl: './historial-carritos.view.scss'
})
export class HistorialCarritosView implements OnInit {
  readonly vm = inject(HistorialCarritosViewModel);
  
  ngOnInit(): void { 
    this.vm.cargarCarritos(); 
  }
}