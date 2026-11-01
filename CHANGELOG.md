# Changelog

All notable changes to this project are documented in this file.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project adheres to
[Semantic Versioning 2.0.0](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `Result<TValue, TError>` pattern in the shared kernel.
- `IAuditableEntity` and `AuditableEntityInterceptor` for `CreatedAt` / `UpdatedAt` timestamps.
- `CancellationToken` support in `IBaseRepository` and `IUnitOfWork`, and through the whole Movies context.
- OpenAPI document (`/openapi/v1.json`) and Scalar API reference (`/scalar`).
- Request localization (`en` default, `en-US`, `es`, `es-419`) with `SharedResource` resource files.
- RFC 7807 `ProblemDetails` responses, global exception handling and localized validation messages.
- Localized OpenAPI operations driven by `<Controller>.<Action>.Summary|Description|Response<Code>` keys.
- Request validation for the Movies endpoints.
- EF Core `InitialCreate` migration and `appsettings.Production.json` with environment-variable connection strings.
- README, architecture decision records and this changelog.

### Changed

- Target framework upgraded to .NET 10 / C# 14; EF Core 10, MySQL provider 10 and Humanizer 3.
- The Movies command service returns `Result<Movie, EMovieError>`; its controller no longer uses `try/catch`.
- Movies service contracts moved from `Domain/Services` to `Application/Services`.
- Repositories that hid base members with `new` now `override` them.
- EF Core logs flow through `ILogger`; the MySQL provider is configured once for every environment.
- Database schema is created with EF Core migrations applied on startup instead of `EnsureCreated()`.
- Scheduling: a double-booked room or an overlapping time slot now returns `409 Conflict` instead of `400`; `400` is reserved for invalid requests.

### Removed

- Swashbuckle (`Swashbuckle.AspNetCore` and `.Annotations`).
- `EntityFrameworkCore.CreatedUpdatedDate` package.

### Fixed

- `UpdateMovie` returns 409 instead of an unhandled database error when the new title already exists.
- `.http` sample request pointed to a non-existent endpoint.
- `EditShow` returned `400` for an unknown show instead of the documented `404`.
- Shows whose end time is not after their start time are rejected instead of being saved.
- Ticketing sync no longer records a successful provider call as failed when saving or event handling throws, no longer records a cancelled request as a provider failure, and truncates provider error messages to the 1000-character column.