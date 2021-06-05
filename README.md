# MyAccess

###### Requerimientos
- Windows / Linux / Mac
- SQL Server 2012 +
- .Net 5
- Visual Studio 2019 + (opcional)

###### Ejecución de la solución
1. Desde de la consola, acceda el directorio "./Providers" dentro de este ejecutar el comando `dotnet ef database update  --connection "[connectionString]"` remplazando `[connectionString]` por uno propio.

1. Con su editor de texto preferido remplace la propiedad `CONNECTION_STRING` dentro del archivo "./WebApplication/Properties/launchSettings.json" por uno propio.

1. Desde de la consola navegar hasta el directorio "./WebApplication" dentro de este ejecutar el comando `dotnet run`.

1. Una vez iniciada la aplicación podrá acceder a la misma desde su navegador de confianza utilizando la siguiente dirección: http://localhost:5000

El usuario y contraseña para ingresar al sistema es:

Usuario: test@myaccess.com
Contraseña: 12345678