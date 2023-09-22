- cd api3 // Caperta del .Csproj
- dotnet tool install --global dotnet-ef // Por si no tiene todos los comandos de entity framework
- dotnet ef migrations add firstmigration --project api3.csproj // Hacer la migracion
- dotnet ef database update firstmigration --project api3.csproj // Actualizar la base de datos

- dotnet ef migrations remove // Por si por alguna razon la migration falla (No deberia)


