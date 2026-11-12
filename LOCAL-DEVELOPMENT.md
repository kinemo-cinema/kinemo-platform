# Ejecutar el backend localmente

Requisitos: .NET SDK 9 y MySQL Server 8.4 o posterior.

1. Inicia tu servidor MySQL.
2. Copia `KinemoPlatform.API/appsettings.Development.local.example.json` como `KinemoPlatform.API/appsettings.Development.local.json`.
3. En la copia local, sustituye `YOUR_MYSQL_PASSWORD` por tu contraseña y ajusta usuario, host y puerto si es necesario. Este archivo está ignorado por Git: no publiques tus credenciales.
4. Si necesitas los datos compartidos, importa `kinemo.sql`, incluido en el repositorio del frontend, antes de arrancar la API. Revisa el destino antes de importarlo: el dump puede reemplazar tablas existentes de `kinemo-platform-wa`.
5. Desde la carpeta de este repositorio ejecuta:

```powershell
dotnet restore
dotnet run --project KinemoPlatform.API --launch-profile http
```

La API escucha en `http://localhost:5089`. Swagger está en `http://localhost:5089/swagger`.

El frontend está en el repositorio `kinemo-cinema/kinemo-cinema-frontend`, en la rama `local-backend-integration`. Ejecuta `npm install` y `npm run dev` desde su carpeta y abre `http://localhost:5173`. Su proxy conecta con esta API en el puerto 5089.

`Start-LocalDatabase.ps1` arranca únicamente la instalación portátil que se preparó en el equipo original, dentro de `.local/mysql`. Esa instalación y sus datos no se incluyen en Git. En otro equipo usa tu propia instalación de MySQL Server.

Mantenimiento y asientos no están implementados. Pagos y sincronización externa necesitan proveedores reales; no se generan resultados simulados. La documentación de integración y las pruebas están en el repositorio del frontend.
