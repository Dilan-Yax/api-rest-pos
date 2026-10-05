# Contexto del Proyecto Backend - ASP.NET Core Web API POS

Contexto y directivas para asistentes de IA (Antigravity, OpenCode, Cursor, Copilot).

## 1. Información General del Proyecto
- **Nombre**: `API REST POS` (`ApiRestPos`)
- **Tipo de Proyecto**: ASP.NET Core Web API (RESTful), target `.NET 10.0`
- **Arquitectura**: En capas — `Controllers` (Attribute Routing) -> `Services` -> `Domain/IRepositories` -> `Persistence/Repositories` -> EF Core. DTOs en `Dtos/`.
- **Persistencia**: EF Core **InMemory** (`UseInMemoryDatabase("ApiRestPosDb")` en `Program.cs:18`). Sin SQL real, sin migraciones; `EnsureCreated()` al iniciar.
- **Autenticación**: JWT Bearer. Clave/Issuer/Audience/Expiry en sección `Jwt` de `appsettings.json`.
- **Documentación**: OpenAPI + Scalar, **solo en Development** (`Program.cs:52`). Rutas: `/scalar/v1`, `/openapi/v1.json`.

## 2. Estructura
```
Api/
├── Api.slnx                    # Solución: incluye ApiRestPos + ApiRestPos.Tests
├── ApiRestPos/
│   ├── Controllers/            # AuthController, ProductsController, PosStatusController, WeatherForecastController
│   ├── Domain/                 # Models/ (Product, Sale, Usuario), IRepositories/, IServices/
│   ├── Dtos/                   # CreateProductDto, UpdateProductDto, LoginDto, RegisterUsuarioDto, AuthResponseDto, ...
│   ├── Persistence/            # Context/PosDbContext, Repositories/
│   ├── Services/               # ProductService, AuthService
│   ├── Utils/                  # JwtTokenService (singleton)
│   └── Program.cs              # Pipeline, DI (scoped servicios/repos), EF InMemory, JWT
└── ApiRestPos.Tests/           # xUnit + Moq; Controller tests con mocks de servicios
```

## 3. Comandos Principales
- **Compilar**: `dotnet build` (desde la raíz de la solución)
- **Ejecutar API**: `dotnet run --project ApiRestPos`
- **Ejecutar tests**: `dotnet test` (xUnit + Moq; usa mocks, no requiere BD). Único archivo: `ApiRestPos.Tests/Controllers/ProductsControllerTests.cs` (29 tests).
- **Imagen Docker**: `docker build -t apirestpos -f ApiRestPos/Dockerfile .`
- **Docs interactivas**: `https://localhost:<puerto>/scalar/v1` (solo Development)

## 4. Estrategia de Ramas (Git Flow)
- `main`: producción. `develop`: integración. `stage`: pruebas/despliegue preliminar.
- `feature-*`: nuevas características. Rama actual de trabajo: `feature-a`.

## 5. Convenciones de Desarrollo
- Nomenclatura C# estándar: PascalCase en clases/métodos, camelCase en locales/parámetros.
- Endpoints bajo `api/[controller]`; retornan códigos HTTP semánticos (`200`, `201`, `400`, `401`, `404`, `204`).
- Servicios y repositorios lanzan `ArgumentException` con mensaje en español para entradas inválidas; los controllers las convierten a `400 BadRequest`.
- Al añadir un service/repo nuevo, registrar su DI (scoped) en `Program.cs`. `JwtTokenService` es singleton.
- Documentar nuevos endpoints (OpenAPI/Scalar).

## 6. Reglas Obligatorias
- **Pruebas unitarias**: toda característica o endpoint nuevo debe incluir pruebas unitarias (xUnit + Moq) en `ApiRestPos.Tests`. Mantener la suite en verde con `dotnet test`.
- **Buenas prácticas de desarrollo**: seguir la arquitectura en capas, validar entradas en servicios/repositorios, evitar lógica en controllers, no agregar dependencias innecesarias y no introducir secretos en código.
- **Documentar todo lo desarrollado**: todo componente/endpoint nuevo debe registrarse en `DOCUMENTATION.md` (sección Componentes) y reflejarse en OpenAPI/Scalar.
- **Documentar cada cambio**: cada cambio al proyecto (feature, fix, refactor, configuración) debe añadir una entrada con fecha en `DOCUMENTATION.md` (sección Historial de Cambios).

## 7. Referencias
- `DOCUMENTATION.md`: registro central de componentes y historial de cambios del proyecto.