# Sistema de Control Clínico Veterinario v2

Aplicación de escritorio Windows Forms en C# (.NET 8) con SQLite. Está adaptada del proyecto de ejemplo entregado, conservando el patrón de inicializador de base de datos, repositorio parametrizado, separación de formularios y manejo de errores.

## Módulos y CRUD
- **Propietarios:** crear, consultar, actualizar y eliminar propietarios.
- **Mascotas:** ficha de mascota, especie, raza, sexo, nacimiento, peso, propietario y observaciones.
- **Veterinarios:** datos de contacto y especialidad.
- **Citas:** agenda, mascota, veterinario, fecha/hora, motivo y estado.
- **Historial clínico:** síntomas, diagnóstico, tratamiento, medicamentos y próxima visita.

## Tablas SQLite
`Propietarios`, `Mascotas`, `Veterinarios`, `Citas` y `ConsultasClinicas`, con claves foráneas, restricciones e índices. La base se crea automáticamente en `%LOCALAPPDATA%/UMG/VeterinariaV2/veterinaria.db`. Para cambiar la ubicación defina la variable de entorno `VETERINARIA_DB` antes de iniciar.

## Requisitos y ejecución
1. Windows 10/11 y .NET 8 SDK.
2. Abra `SistemaControlClinicoVeterinarioV2.csproj` en Visual Studio 2022 o ejecute en la carpeta del proyecto:
   ```powershell
   dotnet restore
   dotnet build
   dotnet run
   ```
3. La primera ejecución crea la base de datos.

## Uso de formularios
Cada pestaña presenta una tabla y un formulario. Seleccione una fila para cargarla en los campos, modifique y pulse **Guardar** para actualizar; para crear, pulse **Nuevo / Limpiar** y después **Guardar**. Para borrar, seleccione una fila y pulse **Eliminar**.

En Mascotas, Citas e Historial clínico se introducen los ID de los registros relacionados. Cree primero propietarios, mascotas y veterinarios; luego registre citas y consultas. Los campos de fecha se escriben como `AAAA-MM-DD`; fecha y hora de cita como `AAAA-MM-DD HH:MM`. Para el peso use punto decimal, por ejemplo `4.5`.

## Archivos principales
- `BaseDatos/crear_tablas.sql`: esquema de base de datos.
- `Persistencia/InicializadorBaseDatos.cs`: crea el archivo y ejecuta el script SQL.
- `Persistencia/RepositorioClinico.cs`: consultas y CRUD con parámetros SQLite.
- `Formularios/DefinicionModulo.cs`: definición de tablas, consultas y campos de cada módulo.
- `Formularios/FrmCrudModulo.cs`: formulario CRUD reutilizable.
- `Formularios/FrmPrincipal.cs`: ventana principal y pestañas.
- `Program.cs`: punto de entrada.

> Nota: este proyecto es una base funcional académica. Antes de usarlo en una clínica real, agregue autenticación y permisos, copias de seguridad, auditoría, validación de solapamiento de citas, protección de datos y pruebas clínicas/operativas. No sustituye el criterio de un veterinario.
