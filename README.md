# ASP.NET Core Web API

Proyecto de ejemplo de una API RESTful utilizando ASP.NET Core. Este proyecto incluye funcionalidades para gestionar productos y categorías, con operaciones CRUD completas.

## Ejecución

Para ejecutar el proyecto, asegúrate de tener .NET SDK instalado y ejecuta el siguiente comando en la terminal:

```bash
dotnet run
```

## Migraciones de la Base de Datos

Para aplicar las migraciones y crear la base de datos, utiliza el siguiente comando:

```bash
dotnet ef database update
```

## Endpoints

- **Productos**
  - `GET /api/products` - Obtener todos los productos
  - `GET /api/products/{id}` - Obtener un producto por ID
  - `POST /api/products` - Crear un nuevo producto
  - `PUT /api/products/{id}` - Actualizar un producto existente
  - `DELETE /api/products/{id}` - Eliminar un producto

- **Categorías**
  - `GET /api/categories` - Obtener todas las categorías
  - `GET /api/categories/{id}` - Obtener una categoría por ID
  - `POST /api/categories` - Crear una nueva categoría
  - `PUT /api/categories/{id}` - Actualizar una categoría existente
  - `DELETE /api/categories/{id}` - Eliminar una categoría
