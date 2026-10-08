/**
 * Un artículo dentro del carrito del cliente (US09 y US10).
 * Sus campos coinciden con el ItemCarritoDto que devuelve la API.
 */
export interface ItemCarrito {
  id: number;
  usuarioId: number;
  productoId: number;
  nombreProducto: string;
  precioUnitario: number;
  cantidad: number;
  subtotal?: number;
}

/**
 * Datos que se envían a la API para agregar un artículo al carrito (US09).
 */
export interface AgregarItemRequest {
  usuarioId: number;
  productoId: number;
  nombreProducto: string;
  precioUnitario: number;
  cantidad: number;
}
