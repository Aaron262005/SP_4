import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccesoProductosService } from '../../services/productos/acceso-productos.service';

/** Impide que Cliente o Auditor abran alta/edición mediante una URL directa. */
export const administradorProductosGuard: CanActivateFn = () =>
  inject(AccesoProductosService).puedeModificar() || inject(Router).createUrlTree(['/productos']);
