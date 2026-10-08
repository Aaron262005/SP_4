/**
 * MODELO de un producto: solo datos, sin lógica.
 * Sus campos coinciden exactamente con el ProductoDto que devuelve la API
 * (el JSON usa los nombres en minúscula inicial).
 * Lo comparten el catálogo (US03) y el alta, edición y eliminación (US06, US07 y US08).
 */
export interface Producto {
  id: number;
  titulo: string;
  precio: number;
  descripcion: string;
  categoria: string;
  imagen: string;
}

/** Datos que el usuario captura al agregar o editar: es un Producto sin el id, que genera el servidor. */
export type GuardarProducto = Omit<Producto, 'id'>;