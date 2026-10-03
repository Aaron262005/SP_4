import { Component, input } from '@angular/core';

/**
 * Alerta roja reutilizable para mostrar errores (por ejemplo, "Usuario o contraseña inválidos").
 * role="alert" hace que los lectores de pantalla anuncien el mensaje automáticamente.
 */
@Component({
  selector: 'app-alerta',
  template: `
    <div class="alerta" role="alert">
      <span class="alerta__icono" aria-hidden="true">!</span>
      <span class="alerta__texto">{{ mensaje() }}</span>
    </div>
  `,
  styles: [
    `
      .alerta {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 12px 14px;
        margin-bottom: 18px;
        border: 1px solid #f04438;
        border-left-width: 5px;
        border-radius: 10px;
        background: #fef3f2;
        color: #b42318;
        font-size: 14px;
        font-weight: 600;
      }
      .alerta__icono {
        flex: none;
        width: 22px;
        height: 22px;
        border-radius: 50%;
        background: #d92d20;
        color: #fff;
        font-size: 14px;
        font-weight: 700;
        line-height: 22px;
        text-align: center;
      }
    `,
  ],
})
export class AlertaComponent {
  /** Texto que se muestra en la alerta (obligatorio). */
  readonly mensaje = input.required<string>();
}