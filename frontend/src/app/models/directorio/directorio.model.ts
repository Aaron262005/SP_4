// Interfaces que soportan la anidación compleja requerida por la HU
export interface Direccion {
  calle: string;
  ciudad: string;
  coordenadas: string;
}

export interface DirectorioUsuario {
  id: number;
  nombreCompleto: string;
  correo: string;
  telefono: string;
  direccion: Direccion;
}