/** Datos compartidos por catálogo, alta, edición y eliminación. */
export interface Producto {
  id: number;
  titulo: string;
  precio: number;
  descripcion: string;
  categoria: string;
  imagen: string;
}
export type GuardarProducto = Omit<Producto, 'id'>;
