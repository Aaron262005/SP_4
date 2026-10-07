# Prueba de integración para ejecutar únicamente contra la API local de desarrollo.
import json, urllib.request, urllib.error, base64, hmac, hashlib, time, concurrent.futures, os
BASE=os.environ.get('TIENDA_TEST_URL','http://localhost:5000')
checks=[]
def req(method,path,data=None,token=None,expected=200):
 headers={'Content-Type':'application/json'}
 if token: headers['Authorization']='Bearer '+token
 r=urllib.request.Request(BASE+path, data=json.dumps(data).encode() if data is not None else None,headers=headers,method=method)
 try:
  with urllib.request.urlopen(r) as res: code=res.status; body=res.read()
 except urllib.error.HTTPError as e: code=e.code; body=e.read()
 assert code==expected,(method,path,code,body[:300])
 checks.append(f'{method} {path}: {code}')
 return json.loads(body) if body else None
admin=req('POST','/auth/login',{'nombreUsuario':'admin.ana','contrasena':'admin123'})['token']
client=req('POST','/auth/login',{'nombreUsuario':'cliente.carlos','contrasena':'cliente123'})['token']
auditor=req('POST','/auth/login',{'nombreUsuario':'auditor.marta','contrasena':'auditor123'})['token']
req('GET','/usuarios/1')
datos={'titulo':'Prueba US06','precio':42.75,'descripcion':'Producto temporal de prueba','categoria':'Pruebas','imagen':'https://example.com/imagen.png'}
before=req('GET','/products')
for method,path in [('POST','/products'),('PUT','/products/101'),('DELETE','/products/101')]:
 for tok,status in [(None,401),('invalid',401),(client,403),(auditor,403)]:
  req(method,path,datos if method!='DELETE' else None,tok,status)
# Alterar la identidad en la carga no concede permisos porque invalida la firma.
parts=client.split('.'); payload=json.loads(base64.urlsafe_b64decode(parts[1]+'==')); payload['sub']='1';parts[1]=base64.urlsafe_b64encode(json.dumps(payload).encode()).decode().rstrip('=')
req('POST','/products',datos,'.'.join(parts),401)
# Token correctamente firmado pero vencido: también rechazado.
parts=admin.split('.'); payload=json.loads(base64.urlsafe_b64decode(parts[1]+'=='));payload['exp']=int(time.time())-1;parts[1]=base64.urlsafe_b64encode(json.dumps(payload).encode()).decode().rstrip('=');parts[2]=base64.urlsafe_b64encode(hmac.new(b'clave-secreta-solo-para-el-trabajo-de-clase-tienda-online-2026',('.'.join(parts[:2])).encode(),hashlib.sha256).digest()).decode().rstrip('=')
req('POST','/products',datos,'.'.join(parts),401)
for campo,valor in [('titulo','   '),('descripcion',''),('categoria',''),('imagen','javascript:alert(1)'),('precio','abc'),('precio',None)]:
 req('POST','/products',{**datos,campo:valor},admin,400)
omitido=dict(datos);del omitido['precio'];req('POST','/products',omitido,admin,400)
created=req('POST','/products',datos,admin,201); pid=created['id']; assert pid not in [p['id'] for p in before]
assert req('GET',f'/products/{pid}')['titulo']==datos['titulo']
updated=req('PUT',f'/products/{pid}',{**datos,'titulo':'Editado US07','precio':67},admin)
assert updated['precio']==67 and req('GET',f'/products/{pid}')['titulo']=='Editado US07'
req('PUT',f'/products/{pid}',{**datos,'titulo':' '},admin,400)
assert req('GET',f'/products/{pid}')['titulo']=='Editado US07'
removed=req('DELETE',f'/products/{pid}',token=admin); assert removed['id']==pid
req('GET',f'/products/{pid}',expected=404); req('DELETE',f'/products/{pid}',token=admin,expected=404)
req('PUT','/products/999999',datos,admin,404)
# IDs únicos con peticiones simultáneas y después de eliminar el ID más alto.
with concurrent.futures.ThreadPoolExecutor(max_workers=8) as pool: created=list(pool.map(lambda _:req('POST','/products',datos,admin,201),range(12)))
ids=[p['id'] for p in created];assert len(set(ids))==12 and min(ids)>pid
for product_id in ids:req('DELETE',f'/products/{product_id}',token=admin)
assert req('GET','/products')==before
print(json.dumps({'resultado':'OK','comprobaciones':len(checks),'detalle':checks},ensure_ascii=False,indent=2))
