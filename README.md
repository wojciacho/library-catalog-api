# LibraryCatalog

A REST API backend for managing a library's book catalog — built with ASP.NET Core as a C#/.NET learning project (from language basics, through layered Web API architecture, JWT authentication, to unit and integration tests).

## Stack

- **.NET 10 / ASP.NET Core Web API**
- **Entity Framework Core** + **SQLite**
- **JWT Bearer authentication**
- **xUnit** + **Moq** — unit tests
- **`WebApplicationFactory`** — integration tests against an in-memory SQLite database
- **Scalar** — interactive API documentation (OpenAPI)
- **`IMemoryCache`** — response caching with explicit invalidation on writes
- **`BackgroundService`** — periodic in-process background job
- **Docker** — multi-stage build for containerized deployment

## Architecture

Layers: `Controller → Service → Repository → DbContext → SQLite`, each depending only on the interface of the layer below it (SOLID, Dependency Injection).

```
Controllers/    → HTTP handling (routing, status codes, [Authorize])
Services/       → business logic
Repositories/   → data access (EF Core)
Data/           → DbContext
Models/         → domain entities, request DTOs
Middleware/     → global exception handling
BackgroundServices/ → periodic background jobs (IHostedService)
Migrations/     → EF Core migrations
LibraryCatalog.Tests/ → unit tests (xUnit + Moq) and integration tests (WebApplicationFactory)
```

## Endpoints

All endpoints below require a `Bearer` token except `POST /Auth/login`.

| Method | Path                | Description                |
|--------|---------------------|-----------------------------|
| POST   | `/Auth/login`       | log in, returns a JWT       |
| GET    | `/Books`            | list all books              |
| GET    | `/Books/{id}`       | get a single book by id     |
| POST   | `/Books`            | create a new book           |
| PUT    | `/Books/{id}`       | update a book                |
| DELETE | `/Books/{id}`       | delete a book                 |
| GET    | `/Members`          | list all members             |
| GET    | `/Members/{id}`     | get a single member by id     |
| POST   | `/Members`          | create a new member           |
| PUT    | `/Members/{id}`     | update a member                |
| DELETE | `/Members/{id}`     | delete a member                 |
| GET    | `/Loans`            | list all loans                  |
| GET    | `/Loans/{id}`       | get a single loan by id         |
| POST   | `/Loans`            | borrow a book (creates a loan)  |
| PUT    | `/Loans/{id}/return`| return a borrowed book          |
| DELETE | `/Loans/{id}`       | delete a loan                    |

Full end-to-end test scenario: [`LibraryCatalog.http`](LibraryCatalog.http).

## Configuration

The JWT signing key is **not** committed to the repository. It's read via `IConfiguration` from the `Jwt:Key` setting, supplied through [.NET User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) locally:

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "<any long random string>"
```

In a real deployment this would come from an environment variable (`Jwt__Key`) or a secret manager instead.

Login credentials are intentionally hardcoded (`admin` / `password`) as a deliberate simplification — a real user database with password hashing is a planned future exercise, not yet implemented.

## Running

```bash
dotnet restore
dotnet ef database update   # creates librarycatalog.db from migrations
dotnet run
```

The API starts at the address printed in the console (defaults to `http://localhost:5147`). Interactive docs at `/scalar/v1` in development.

## Docker

```bash
docker build -t library-catalog-api .
docker run -d -p 8080:8080 -e Jwt__Key="<any long random string>" library-catalog-api
```

EF Core migrations are applied automatically at startup (`Database.Migrate()` in `Program.cs`), so the container creates its own SQLite schema on first run — no manual migration step needed inside the container. The JWT key is passed as an environment variable (`Jwt__Key`, double underscore — the `IConfiguration` convention for nested keys) rather than baked into the image.

## Tests

```bash
dotnet test
```

Unit tests (service layer, mocked dependencies) and integration tests (full HTTP pipeline via `WebApplicationFactory`, in-memory SQLite database).

## Status

Learning project — actively evolving, new stages added as I go.
