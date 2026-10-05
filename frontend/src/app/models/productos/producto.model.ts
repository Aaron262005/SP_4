/**
 * MODELO de un producto: solo datos, sin lógica.
 * Sus campos coinciden exactamente con el ProductoDto que devuelve la API
 * (el JSON usa los nombres en minúscula inicial).
 */
export interface Producto {
  id: number;
  titulo: string;
  precio: number;
  descripcion: string;
  categoria: string;
  imagen: string;
}