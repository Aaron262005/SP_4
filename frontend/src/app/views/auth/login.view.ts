import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AlertaComponent } from '../../shared/alerta/alerta.component';
import { LoginViewModel } from '../../viewmodels/auth/login.viewmodel';

/**
 * VIEW de la pantalla de Login.
 * Solo presenta la interfaz y delega todo al ViewModel.
 * "providers" crea una instancia nueva del ViewModel para esta pantalla.
 */
@Component({
  selector: 'app-login',
  imports: [FormsModule, AlertaComponent],
  providers: [LoginViewModel],
  templateUrl: './login.view.html',
  styleUrl: './login.view.scss',
})
export class LoginView {
  readonly vm = inject(LoginViewModel);
}