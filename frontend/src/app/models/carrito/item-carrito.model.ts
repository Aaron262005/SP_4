/** Un producto dentro del carrito de compras. */
export interface ItemCarrito {
  productoId: number;
  nombre: string;
  precio: number;
  cantidad: number;
}