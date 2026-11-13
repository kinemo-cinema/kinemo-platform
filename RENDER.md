# Kinemo en Render Free

La imagen Docker compila el frontend Vue y el backend .NET 10. Ambos se sirven desde una sola dirección HTTPS: la página en `/`, la API en `/api/v1`, la documentación en `/scalar` y la comprobación de conexión a la base en `/health`.

## Configuración del servicio

- Runtime: Docker; Dockerfile: `./Dockerfile`; contexto: `.`.
- Plan: Free. La base PostgreSQL y el servicio deben compartir región y workspace.
- `ASPNETCORE_ENVIRONMENT=Production`
- `Database__Provider=PostgreSQL`
- `ConnectionStrings__DefaultConnection`: URL **interna** de PostgreSQL, guardada como variable privada de Render.
- `Hosting__UseHttpsRedirection=false`: Render termina TLS y redirige el tráfico público a HTTPS.
- `PORT=10000`

El `Dockerfile` fija `FRONTEND_REF` a un commit del repositorio público del frontend. Para publicar cambios posteriores del frontend, actualiza esa referencia y vuelve a desplegar el backend. El frontend usa `/api/v1`, sin direcciones de localhost ni contraseñas.

## Datos

PostgreSQL tiene un contexto y migraciones propios; MySQL conserva sus migraciones y sigue siendo el proveedor local predeterminado. No se importan los datos del antiguo fake backend.

La variable opcional `Database__BootstrapSnapshot` permite trasladar una copia JSON comprimida con gzip y codificada en base64 a una base **vacía**. La importación valida las tablas contra el modelo, respeta las dependencias, conserva identificadores y ajusta las secuencias. Se ejecuta en una transacción y registra la huella de la copia para evitar duplicados en reinicios. Se niega a sobrescribir una base con registros. Retira esa variable privada tras verificar la importación. Nunca subas copias con datos ni credenciales al repositorio.

Los archivos sensoriales de la versión PostgreSQL se guardan en la base, porque el disco de Render Free es temporal. El entorno MySQL continúa usando archivos locales.

Para comprobar escrituras sin tocar los registros de la aplicación, `Database__RunSmokeCheck=true` ejecuta una verificación al arrancar dentro de un esquema temporal propio. Comprueba migraciones, altas y edición, relaciones sensoriales, fechas con zona horaria y subida/lectura/borrado de archivos. Elimina únicamente ese esquema temporal al terminar. Retira o desactiva la variable tras comprobar el resultado en los logs.

## Límites de esta instalación gratuita

La base creada para esta exposición caduca el **13 de diciembre de 2026**. Exporta los datos y prepara otra base antes de esa fecha si necesitan seguir utilizándola. El servicio web gratuito puede dormirse cuando no tiene visitas; la primera carga puede tardar mientras se reactiva.

Para revisar el despliegue: `/health` debe responder `200` con `{"status":"ok"}`; `/api/v1/movies` debe devolver los registros reales; una ruta como `/catalog/movies` debe abrir directamente la aplicación. Una dirección API inexistente debe devolver `404`, sin devolver el HTML del frontend.
