# Kinemo Platform API

RESTful backend of **Kinemo**, the B2B SaaS platform by *VonNeuman New Men* that lets small and medium cinema
chains operate 4D rooms (motion seats, wind, vibration): movie and sensory content catalog, show scheduling,
room readiness, seat control, effects execution, maintenance, analytics and subscriptions.

> UPC · Ingeniería de Software · Aplicaciones Web (1ASI0730) · 2026-20

## Tech stack

| Area | Choice |
|---|---|
| Runtime | .NET 10, C# 14 (SDK pinned in `global.json`) |
| Web | ASP.NET Core Web API (controllers) |
| Persistence | Entity Framework Core 10 + MySQL (`MySql.EntityFrameworkCore`) |
| API documentation | Built-in OpenAPI document + [Scalar](https://scalar.com/) |
| Domain events | Cortex.Mediator |
| i18n | `IStringLocalizer` + `.resx` (default English, Spanish) |

## Getting started

```bash
# 1. Prerequisites: .NET 10 SDK and a running MySQL server
dotnet --version            # 10.0.x

# 2. Configure the connection string (do not commit real credentials)
cd KinemoPlatform.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "server=localhost;port=3306;user=root;password=<your-password>;database=kinemo-platform-wa"

# 3. Run (migrations are applied on startup)
dotnet run
```

| URL | What |
|---|---|
| `/scalar` | Interactive API reference (Scalar) |
| `/openapi/v1.json` | OpenAPI document |

Production settings read the connection string from the `DATABASE_URL`, `DATABASE_USER`,
`DATABASE_PASSWORD` and `DATABASE_SCHEMA` environment variables (`appsettings.Production.json`).

## Internationalization

Send an `Accept-Language` header. Supported cultures: `en` (default), `en-US`, `es`, `es-419`; regional
variants such as `es-PE` fall back to their parent culture. Messages live in
`KinemoPlatform.API/Resources/SharedResource*.resx`; a key missing in a language falls back to English.

Validation errors and exceptions are returned as `application/problem+json` (RFC 7807).

## Database migrations

```bash
# one-time: install the EF Core CLI
dotnet tool install --global dotnet-ef

# from the repository root
dotnet ef migrations add <MigrationName> --project KinemoPlatform.API --output-dir Migrations
```

Migrations are applied automatically when the API starts.

## Project structure

The API is a single deployable project organized by bounded context (see [docs/adrs.md](docs/adrs.md)):

```text
KinemoPlatform.API/
  Shared/          Cross-cutting kernel: Result, auditing, repositories, OpenAPI and routing configuration
  Resources/       Localized messages (SharedResource*.resx)
  Movies/          BC01  Movie & sensory content management
  Scheduling/      BC02  Scheduling & calendar
  Rooms/           BC03  Room & resource readiness
  Ticketing/       BC04  Ticketing integration
  Seats/           BC05  Seat allocation & control
  Execution/       BC06  4D execution & synchronization
  Maintenance/     BC07  Resource testing & maintenance
  Analytics/       BC08  Operational analytics & reporting
  Subscriptions/   BC09  Subscription & service management
```

Each bounded context follows the same layers: `Domain` (aggregates, commands, queries, events, repository
contracts), `Application` (service contracts, internal services, errors), `Infrastructure` (EF Core, external
services) and `Interfaces/REST` (controllers, resources, assemblers).

## Contributing

* Branching: **GitFlow** (`main`, `develop`, `feature/*`, `release/*`, `hotfix/*`).
* Commits: **Conventional Commits** (`feat`, `fix`, `docs`, `refactor`, `build`, `chore`, ...).
* Versioning: **Semantic Versioning 2.0.0**; notable changes are recorded in [CHANGELOG.md](CHANGELOG.md).