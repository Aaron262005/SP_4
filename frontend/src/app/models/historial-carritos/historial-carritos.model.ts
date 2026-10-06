export interface ItemHistorial { 
    productoId: number; 
    tituloProducto: string; 
    cantidad: number; 
}

export interface HistorialCarrito { 
    id: number; 
    usuarioId: number; 
    fechaCreacion: string; 
    items: ItemHistorial[]; 
}