/** Un producto dentro del carrito de compras. */
export interface ItemCarrito {
  productoId: number;
  nombre: string;
  precio: number;
  cantidad: number;
}

/**
 * Interfaz que representa la estructura de un artículo en el carrito del cliente.
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
 * DTO de solicitud HTTP para agregar un ítem al carrito (US09).
 */
export interface AgregarItemRequest {
  usuarioId: number;
  productoId: number;
  nombreProducto: string;
  precioUnitario: number;
  cantidad: number;
}