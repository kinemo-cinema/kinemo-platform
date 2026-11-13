# Ejecutar el backend localmente

Requisitos: .NET SDK 10 y MySQL Server 8.4 o posterior.

1. Inicia tu servidor MySQL.
2. Copia `KinemoPlatform.API/appsettings.Development.local.example.json` como `KinemoPlatform.API/appsettings.Development.local.json`.
3. En la copia local, sustituye `YOUR_MYSQL_PASSWORD` por tu contraseña y ajusta usuario, host y puerto si es necesario. Este archivo está ignorado por Git: no publiques tus credenciales.
4. Si necesitas los datos compartidos, importa `kinemo.sql`, incluido en el repositorio del frontend, antes de arrancar la API. Revisa el destino antes de importarlo: el dump puede reemplazar tablas existentes de `kinemo-platform-wa`.
5. Desde la carpeta de este repositorio ejecuta:

```powershell
dotnet restore
dotnet run --project KinemoPlatform.API --launch-profile http
```

La API escucha en `http://localhost:5089`. La referencia de la API está en `http://localhost:5089/scalar` y el documento OpenAPI en `http://localhost:5089/openapi/v1.json`.

El frontend está en el repositorio `kinemo-cinema/kinemo-cinema-frontend`, en la rama `develop`. Ejecuta `npm install` y `npm run dev` desde su carpeta y abre `http://localhost:5173`. Su proxy conecta con esta API en el puerto 5089.

`Start-LocalDatabase.ps1` arranca únicamente la instalación portátil que se preparó en el equipo original, dentro de `.local/mysql`. Esa instalación y sus datos no se incluyen en Git. En otro equipo usa tu propia instalación de MySQL Server.

Mantenimiento y asientos se incluyen desde develop. Pagos y sincronización externa necesitan proveedores reales; no se generan resultados simulados. La documentación de integración y las pruebas están en el repositorio del frontend.

## Equipo original con SDK local

En el equipo original puedes ejecutar `./Start-LocalBackend.ps1`: utiliza el SDK 10 instalado en `.local/dotnet10`. En otras laptops usa el SDK 10 instalado normalmente. El script no contiene credenciales.

## Bases de la integración anterior

Esta versión aplica migraciones con EF Core. No arranques directamente contra una base antigua creada mediante EnsureCreated que no tenga historial de migraciones: conserva un respaldo y revisa la compatibilidad del esquema antes de adoptar la migración inicial. La base del equipo original se actualizó conservando todas sus filas; el nuevo `kinemo.sql` incluye el historial correcto y las tablas nuevas. Para otros equipos es preferible importarlo en una base nueva, evitando sobrescribir datos existentes.
