# InvoicesApp

Este repositorio contiene una aplicación básica ASP.NET Core MVC para crear facturas y filtrarlas.

## Requisitos Previos

Asegúrate de tener instalados los siguientes requisitos antes de ejecutar el proyecto:

- [.NET SDK](https://dotnet.microsoft.com/download) - versión 6.0 o superior
- [SQL Server](https://www.microsoft.com/es-co/sql-server/sql-server-downloads)

## Configuración del Proyecto

1. Clona este repositorio:

   ```bash
   git clone https://github.com/lkto/tes-dvp.git
   ```

2. Accede al directorio del proyecto:

   ```bash
   cd tes-dvp
   ```

3. Configura la cadena de conexión en el archivo de configuración `appsettings.json`.

4. Ejecuta el esquema de la base de datos que se encuentra en el archivo [db_schema.sql](Data/db_schema.sql).

## Ejecución del Proyecto

1. Ejecuta la aplicación:

   ```bash
   dotnet run
   ```

   La aplicación estará disponible en http://localhost:7001 (o el puerto que se especifique en la salida).

&nbsp;
