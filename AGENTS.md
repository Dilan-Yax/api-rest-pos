# Contexto del Proyecto Backend - ASP.NET Core Web API POS

Este archivo proporciona contexto y directivas para herramientas y asistentes de inteligencia artificial (Antigravity, OpenCode, Cursor, Copilot).

## 1. Información General del Proyecto
- **Nombre**: `API REST POS` (`ApiRestPos`)
- **Tipo de Proyecto**: ASP.NET Core Web API (RESTful)
- **Plataforma / Framework**: .NET 9 / .NET 10 en C#
- **Arquitectura**: Modelo - Controlador (Controllers con Attribute Routing)
- **Documentación de API**: OpenAPI con interfaz interactiva Scalar (`/scalar/v1`)
- **Soporte de Contenedores**: Dockerfile multi-stage incluido
- **Seguridad**: Redirección HTTPS habilitada

## 2. Estructura de Directorios
```
Api/
├── Api.slnx                       # Solución de Visual Studio 2026
├── .dockerignore                  # Exclusiones para compilación en Docker
├── .gitignore                     # Exclusiones de control de versiones Git
├── AGENTS.md                      # Contexto para asistentes de IA
└── ApiRestPos/
    ├── Controllers/               # Controladores de la API (ProductsController, PosStatusController, WeatherForecastController)
    ├── Models/                    # Modelos de dominio POS (Product, Sale, SaleItem)
    ├── Properties/                # Configuración de ejecución (launchSettings.json)
    ├── ApiRestPos.csproj          # Archivo de proyecto y dependencias NuGet
    ├── Dockerfile                 # Contenedor multi-stage optimizado
    ├── Program.cs                 # Configuración del pipeline y servicios de la API
    └── appsettings.json           # Configuración de la aplicación y logs
```

## 3. Comandos Principales
- **Compilar proyecto**: `dotnet build`
- **Ejecutar API en desarrollo**: `dotnet run --project ApiRestPos`
- **Compilar imagen Docker**: `docker build -t apirestpos -f ApiRestPos/Dockerfile .`
- **Ruta de documentación interactiva**: `https://localhost:<puerto>/scalar/v1`
- **Ruta del esquema OpenAPI**: `https://localhost:<puerto>/openapi/v1.json`

## 4. Estrategia de Ramas (Git Flow)
- `main`: Rama de producción principal.
- `develop`: Rama base de desarrollo e integración.
- `stage`: Rama de pruebas y despliegue preliminar.
- `feature-*` (ej. `feature-a`, `feature-products`, `feature-sales`): Ramas para nuevas características o endpoints.

## 5. Convenciones de Desarrollo
- Seguir las convenciones estándar de nomenclatura en C# (PascalCase para clases/métodos, camelCase para variables locales/parámetros).
- Cada endpoint debe retornar códigos de estado HTTP semánticos (`200 OK`, `201 Created`, `400 BadRequest`, `404 NotFound`).
- Documentar nuevos endpoints para que se reflejen en OpenAPI y Scalar.
