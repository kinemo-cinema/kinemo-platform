# Architecture Decision Records

Format: *Status · Context · Decision · Consequences*. Newest decisions are appended at the end.
Dates use `YYYY-MM-DD`.

## ADR-001 — Single deployable API organized by bounded context

* **Status:** Accepted (2026-10-30)
* **Context:** The domain was split into 9 bounded contexts through EventStorming (BC01–BC09). A separate
  deployable per context would add operational cost that a university project and a small-cinema product do
  not need yet.
* **Decision:** One ASP.NET Core project, `KinemoPlatform.API`, with one top-level folder per bounded
  context. Each context follows `Domain / Application / Infrastructure / Interfaces.REST`. No context imports
  another one; cross-context needs are expressed through domain events.
* **Consequences:** Simple build, run and deployment. All contexts currently share **one `AppDbContext` and one
  database**, which is *not* a database-per-service layout. The report must describe this accurately, or the
  persistence must be split per context in a future decision.

## ADR-002 — Built-in OpenAPI and Scalar instead of Swashbuckle

* **Status:** Accepted (2026-10-30)
* **Context:** Swashbuckle 9 depends on Microsoft.OpenApi 1.x, which conflicts with the OpenAPI generator
  that ships with .NET 10 (Microsoft.OpenApi 2.x). The course reference project also uses OpenAPI + Scalar.
* **Decision:** Generate the document with `AddOpenApi` / `MapOpenApi` (`/openapi/v1.json`) and serve the
  interactive reference with `Scalar.AspNetCore` (`/scalar`). Controllers use `[Tags]`, `[EndpointSummary]`,
  `[EndpointDescription]` and `[ProducesResponseType]`. Localized text is applied by an operation transformer.
* **Consequences:** No Swagger UI. The documentation is available in every environment; wrap
  `MapOpenApi()` / `MapScalarApiReference()` in an environment check to restrict it.

## ADR-003 — `Result<TValue, TError>` for expected application failures

* **Status:** Accepted (2026-10-30). Rollout in progress: Movies is converted; the other contexts follow.
* **Context:** Command services signalled failures with `InvalidOperationException` and nullable returns, and
  controllers translated *every* such exception into 404 or 409 by catching it. A `null` could mean "bad
  request" or "not found" depending on the endpoint.
* **Decision:** Application services return `Result<TValue, TError>` where `TError` is an enum per bounded
  context (e.g. `EMovieError`). An assembler in `Interfaces/REST/Transform` maps each error to an HTTP status.
  Unexpected failures are not caught in services.
* **Consequences:** Failure modes are explicit in method signatures and testable without exceptions.
  Controllers lose their `try/catch` blocks. Until every context is converted, the codebase mixes both styles.

## ADR-004 — RFC 7807 ProblemDetails and global exception handling

* **Status:** Accepted (2026-10-30)
* **Context:** Errors used ad-hoc shapes (`{ message }`, empty bodies, stack traces in development).
* **Decision:** `AddProblemDetails` + `UseExceptionHandler`, a custom `InvalidModelStateResponseFactory`, and
  `ControllerBase.Problem(...)` for domain failures. Every error is `application/problem+json`.
* **Consequences:** One error shape for clients. A race on a unique index still surfaces as a 500
  ProblemDetails; handle it explicitly where it matters.

## ADR-005 — Localization with resource files (`en` default, `es`, `es-419`)

* **Status:** Accepted (2026-10-30). Rollout in progress: OpenAPI text is translated for Movies only.
* **Context:** The rubric requires English and Latin American Spanish with English as default. The course
  reference project uses `en`, `en-US`, `es`, `es-PE`.
* **Decision:** `IStringLocalizer<SharedResource>` with `Resources/SharedResource*.resx`. Supported cultures:
  `en` (default), `en-US`, `es`, `es-419`; regional variants fall back to the parent culture. Validation
  messages are resource keys. English summaries/descriptions stay on the endpoints; resource keys
  `<Controller>.<Action>.Summary|Description|Response<Code>` override them per culture.
* **Consequences:** Translating an endpoint only requires adding keys. Logs stay in English. Endpoints
  without Spanish keys show their English text.

## ADR-006 — EF Core migrations applied on startup

* **Status:** Accepted (2026-10-30)
* **Context:** `EnsureCreated()` cannot evolve an existing schema.
* **Decision:** Commit migrations and call `Database.Migrate()` at startup. Connection strings may use
  `%ENV_VARS%` expanded at runtime.
* **Consequences:** Schema changes are versioned and reviewable. Developers must drop databases that were
  created with `EnsureCreated()` once. Applying migrations on startup suits a single instance; revisit before
  scaling out.

## ADR-007 — Audit timestamps through a SaveChanges interceptor

* **Status:** Accepted (2026-10-30)
* **Context:** `EntityFrameworkCore.CreatedUpdatedDate` targets EF Core 8 and is unmaintained for EF Core 10.
* **Decision:** Aggregates implement `IAuditableEntity` (`CreatedAt`, `UpdatedAt`). A single shared
  `AuditableEntityInterceptor` fills them when changes are saved.
* **Consequences:** No third-party dependency; column names are unchanged (`created_at`, `updated_at`).

## ADR-008 — Domain events with Cortex.Mediator

* **Status:** Proposed (2026-10-30) — confirm with the course instructor
* **Context:** The EventStorming model defines domain events, implemented with Cortex.Mediator. The course
  reference project does not use a mediator. Current handlers only log.
* **Decision:** Keep the events and handlers; log through `ILogger` rather than `Console.WriteLine`.
  Remove the unused `LoggingCommandBehavior`.
* **Consequences:** Keeps the model consistent with the report. If the instructor requires the reference
  stack, remove the package and publish events from the application services directly.