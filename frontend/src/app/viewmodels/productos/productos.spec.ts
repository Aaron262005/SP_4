import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ActivatedRoute, Router, convertToParamMap, provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { SessionService } from '../../services/auth/session.service';
import { ProductosService } from '../../services/productos/productos.service';
import { AvisoProductosService } from '../../services/productos/aviso-productos.service';
import { FormularioProductoViewModel } from './formulario-producto.viewmodel';
import { DetalleProductoViewModel } from './detalle-producto.viewmodel';
import { CatalogoViewModel } from './catalogo.viewmodel';
import { administradorProductosGuard } from '../../core/guards/administrador-productos.guard';
import { API_URL } from '../../core/api.config';

/** Pruebas de aceptación locales: validación, permisos, cancelación y efectos de cada respuesta HTTP. */
describe('US06-US07-US08 productos', () => {
  let http: HttpTestingController;
  let servicio: ProductosService;
  let sesion: SessionService;
  const producto = { id: 101, titulo: 'Teclado', precio: 25, descripcion: 'Mecánico', categoria: 'Accesorios', imagen: 'https://example.com/a.png' };
  function iniciar(id = 1, rol: 'Administrador' | 'Cliente' | 'Auditor' = 'Administrador', exp = Date.now() / 1000 + 600) {
    const token = 'e30.' + btoa(JSON.stringify({ sub: String(id), exp })) + '.firma';
    sesion.guardar({ token, usuarioId: id, rol, nombre: 'Prueba' });
  }
  function preparar(id: string | null = null) {
    localStorage.clear();
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]),
      FormularioProductoViewModel, DetalleProductoViewModel, CatalogoViewModel,
      { provide: ActivatedRoute, useValue: { snapshot: { paramMap: convertToParamMap(id ? { id } : {}) } } },
    ] });
    http = TestBed.inject(HttpTestingController); servicio = TestBed.inject(ProductosService); sesion = TestBed.inject(SessionService);
    iniciar();
  }
  afterEach(() => { http?.verify(); localStorage.clear(); TestBed.resetTestingModule(); });

  it('US06 bloquea campos vacíos sin enviar HTTP', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel); vm.guardar();
    expect(vm.invalido('titulo')).toBe(true); expect(vm.error()).toContain('Corrige');
    http.expectNone(`${API_URL}/products`);
  });
  it.each(['javascript:alert(1)', 'sin-url', 'ftp://example.com/a.png'])('US06 rechaza URL inválida %s', imagen => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel); vm.formulario.patchValue({ ...producto, imagen }); vm.guardar();
    expect(vm.invalido('imagen')).toBe(true); http.expectNone(`${API_URL}/products`);
  });
  it('US06 rechaza texto de espacios y precio ausente', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel);
    vm.formulario.patchValue({ ...producto, titulo: '   ', precio: null }); vm.guardar();
    expect(vm.invalido('titulo')).toBe(true); expect(vm.invalido('precio')).toBe(true);
  });
  it('US06 rechaza precio no finito', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel);
    vm.formulario.patchValue({ ...producto, precio: NaN }); vm.guardar(); expect(vm.invalido('precio')).toBe(true);
  });
  it('US06 crea una sola vez, envía token, muestra ID y limpia el formulario', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel); vm.formulario.patchValue(producto);
    vm.guardar(); vm.guardar(); const peticion = http.expectOne(`${API_URL}/products`);
    expect(peticion.request.method).toBe('POST'); expect(peticion.request.headers.get('Authorization')).toContain('Bearer ');
    expect(vm.guardando()).toBe(true); peticion.flush({ ...producto, id: 104 }, { status: 201, statusText: 'Created' });
    expect(vm.mensaje()).toContain('104'); expect(vm.formulario.controls.titulo.value).toBe('');
    expect(vm.formulario.controls.titulo.touched).toBe(false); expect(vm.guardando()).toBe(false);
  });
  it('US06 conserva los datos al fallar la API y permite reintentar', () => {
    preparar(); const vm = TestBed.inject(FormularioProductoViewModel); vm.formulario.patchValue(producto); vm.guardar();
    http.expectOne(`${API_URL}/products`).flush({}, { status: 500, statusText: 'Error' });
    expect(vm.guardando()).toBe(false); expect(vm.formulario.controls.titulo.value).toBe(producto.titulo);
    vm.guardar(); http.expectOne(`${API_URL}/products`).flush(producto);
  });
  it.each([[4, 'Cliente'], [3, 'Auditor']] as const)('bloquea POST/PUT/DELETE para %s %s antes de la red', (id, rol) => {
    preparar(); iniciar(id, rol); let errores = 0;
    [servicio.agregar(producto), servicio.editar(101, producto), servicio.eliminar(101)].forEach(p => p.subscribe({ error: () => errores++ }));
    expect(errores).toBe(3); http.expectNone(r => r.url.includes('/products'));
  });
  it('no basta cambiar el rol local de un cliente', () => {
    preparar(); iniciar(4, 'Administrador'); let fallo = false;
    servicio.eliminar(101).subscribe({ error: () => fallo = true }); expect(fallo).toBe(true);
  });
  it('no envía escrituras con sesión vencida', () => {
    preparar(); iniciar(1, 'Administrador', 1); let fallo = false;
    servicio.agregar(producto).subscribe({ error: () => fallo = true }); expect(fallo).toBe(true);
  });
  it('redirige a catálogo en enlaces profundos sin permisos', () => {
    preparar(); iniciar(3, 'Auditor');
    const resultado = TestBed.runInInjectionContext(() => administradorProductosGuard({} as never, {} as never));
    expect(String(resultado)).toBe('/productos');
  });
  it('US07 precarga todos los campos y refleja el éxito al regresar al detalle', () => {
    preparar('101'); const vm = TestBed.inject(FormularioProductoViewModel); const router = TestBed.inject(Router);
    const navegar = vi.spyOn(router, 'navigate').mockResolvedValue(true);
    vm.cargar(); http.expectOne(`${API_URL}/products/101`).flush(producto);
    expect(vm.formulario.controls.titulo.value).toBe(producto.titulo); expect(vm.formulario.controls.imagen.value).toBe(producto.imagen);
    vm.formulario.controls.precio.setValue(99); vm.guardar(); vm.guardar();
    const peticion = http.expectOne(`${API_URL}/products/101`); expect(peticion.request.method).toBe('PUT'); expect(peticion.request.body.precio).toBe(99);
    peticion.flush({ ...producto, precio: 99 }); expect(navegar).toHaveBeenCalledWith(['/productos', 101]);
    expect(TestBed.inject(AvisoProductosService).mensaje()).toBe('Producto actualizado (Simulación)');
  });
  it('US07 no permite guardar un producto que no existe', () => {
    preparar('999'); const vm = TestBed.inject(FormularioProductoViewModel); vm.cargar();
    http.expectOne(`${API_URL}/products/999`).flush({}, { status: 404, statusText: 'Not Found' });
    vm.guardar(); expect(vm.listo()).toBe(false); expect(vm.error()).toContain('no existe');
  });
  it('US08 cancelar no envía DELETE ni altera la vista', () => {
    preparar('101'); const vm = TestBed.inject(DetalleProductoViewModel); vm.cargar();
    http.expectOne(`${API_URL}/products/101`).flush(producto); vm.eliminar(false);
    expect(vm.producto()).toEqual(producto); expect(vm.eliminando()).toBe(false); http.expectNone(r => r.method === 'DELETE');
  });
  it('US08 confirmar elimina una vez, conserva aviso y vuelve al catálogo', () => {
    preparar('101'); const vm = TestBed.inject(DetalleProductoViewModel);
    const navegar = vi.spyOn(TestBed.inject(Router), 'navigate').mockResolvedValue(true);
    vm.cargar(); http.expectOne(`${API_URL}/products/101`).flush(producto); vm.eliminar(true); vm.eliminar(true);
    const peticion = http.expectOne(`${API_URL}/products/101`); expect(peticion.request.method).toBe('DELETE'); peticion.flush(producto);
    expect(navegar).toHaveBeenCalledWith(['/productos']); expect(TestBed.inject(AvisoProductosService).mensaje()).toContain('eliminado');
  });
  it('US08 error de borrado conserva el producto y habilita reintento', () => {
    preparar('101'); const vm = TestBed.inject(DetalleProductoViewModel); vm.cargar(); http.expectOne(`${API_URL}/products/101`).flush(producto);
    vm.eliminar(true); http.expectOne(`${API_URL}/products/101`).flush({}, { status: 500, statusText: 'Error' });
    expect(vm.eliminando()).toBe(false); expect(vm.producto()).toEqual(producto); expect(vm.error()).toContain('operación');
  });
  it('catálogo muestra error y se recupera al reintentar', () => {
    preparar(); const vm = TestBed.inject(CatalogoViewModel); vm.cargar(); http.expectOne(`${API_URL}/products`).error(new ProgressEvent('error'));
    expect(vm.error()).toContain('conectar'); expect(vm.cargando()).toBe(false);
    vm.cargar(); http.expectOne(`${API_URL}/products`).flush([producto]); expect(vm.productos()).toEqual([producto]); expect(vm.error()).toBe('');
  });
});
