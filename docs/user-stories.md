# Kinemo — User Stories

## Alcance y origen

Historias redactadas a partir del backend de [Kinemo Platform](https://github.com/kinemo-cinema/kinemo-platform), rama `develop`, commit `d4b4c617061bb6729c7c3f8dbc42baad04f84df5`, revisado el 11 de noviembre de 2026. Complementan `kinemo-class-diagram.puml`.

Este documento adapta el formato de los adjuntos de ACME a Kinemo. Las historias se reconstruyeron desde el código revisado: **no son una transcripción de los requisitos originales de Kinemo ni una certificación de cumplimiento de estos**. Los roles y beneficios son una redacción propuesta; no acreditan permisos de acceso implementados.

La numeración KIN-US-001 a KIN-US-044 es propia de este entregable. No sustituye ni pretende reproducir referencias US01/US07 u otras presentes en comentarios del repositorio.

## Estado de las historias

- **Implementación en código:** el servicio o dominio revisado contiene el comportamiento descrito. No significa que se hayan ejecutado pruebas de aceptación o validado el frontend.
- **Simulada o parcial:** hay un flujo de backend, pero una parte relevante utiliza datos simulados, registra solo metadatos o no alcanza hardware/proveedores reales.
- Las limitaciones de cada historia forman parte de su alcance. No se incorporan requisitos pendientes como si ya estuvieran implementados.
- Los criterios describen el nivel de servicio/dominio. “Ausencia de resultado” no presupone un código HTTP específico; listados vacíos se distinguen de consultas individuales sin resultado.
- Los datos “admitidos” se refieren a los campos aceptados por el flujo revisado. No se añaden longitudes, rangos o políticas no comprobados.
- Ningún rol de estas historias presupone IAM o autorización por roles implementada.

## Requirement Traceability Matrix (RTM)

| Historia | Funcionalidad | BC | Elementos del backend | Estado |
|---|---|---|---|---|
| [KIN-US-001](#kin-us-001) | Registrar película | BC01 | `Movie`, `CreateMovieCommand`, `MovieCommandService` | Implementación en código |
| [KIN-US-002](#kin-us-002) | Actualizar película | BC01 | `Movie`, `UpdateMovieCommand`, `MovieCommandService` | Implementación en código |
| [KIN-US-003](#kin-us-003) | Consultar y buscar películas | BC01 | `Movie`, `MovieQueryService`, `GetAllMoviesQuery`, `SearchMoviesQuery`, `GetMovieByIdQuery` | Implementación en código |
| [KIN-US-004](#kin-us-004) | Asociar pista sensorial | BC01 | `Movie`, `SensoryContent`, `SensoryTrack`, `AssociateSensoryFileCommand` | Implementación en código |
| [KIN-US-005](#kin-us-005) | Configurar intensidad de efectos | BC01 | `Movie`, `SensoryContent`, `ConfigurationHistoryEntry`, `ConfigureEffectsProfileCommand` | Implementación en código |
| [KIN-US-006](#kin-us-006) | Consultar configuración sensorial | BC01 | `SensoryContent`, `SensoryTrack`, `ConfigurationHistoryEntry`, `GetSensoryContentByMovieIdQuery`, `MovieQueryService` | Implementación en código |
| [KIN-US-007](#kin-us-007) | Desactivar película | BC01 | `Movie`, `DeactivateMovieCommand`, `MovieCommandService` | Implementación en código |
| [KIN-US-008](#kin-us-008) | Programar función | BC02 | `Show`, `TimeSlot`, `ScheduleShowCommand`, `ShowCommandService` | Implementación en código |
| [KIN-US-009](#kin-us-009) | Reprogramar función | BC02 | `Show`, `TimeSlot`, `EditShowCommand`, `ShowCommandService` | Implementación en código |
| [KIN-US-010](#kin-us-010) | Cancelar función | BC02 | `Show`, `CancelShowCommand`, `ShowCommandService` | Implementación en código |
| [KIN-US-011](#kin-us-011) | Consultar calendario | BC02 | `Show`, `ShowQueryService`, `GetDailyScheduleQuery` | Implementación en código |
| [KIN-US-012](#kin-us-012) | Registrar sala | BC03 | `Room`, `CreateRoomCommand`, `RoomCommandService` | Implementación en código |
| [KIN-US-013](#kin-us-013) | Preparar sala | BC03 | `Room`, `RoomInspection`, `PrepareRoomCommand` | Simulada o parcial |
| [KIN-US-014](#kin-us-014) | Bloquear sala | BC03 | `Room`, `RoomBlock`, `BlockRoomCommand` | Implementación en código |
| [KIN-US-015](#kin-us-015) | Liberar sala | BC03 | `Room`, `RoomBlock`, `ReleaseRoomCommand` | Implementación en código |
| [KIN-US-016](#kin-us-016) | Consultar salas | BC03 | `Room`, `RoomInspection`, `RoomBlock`, `RoomQueryService` | Implementación en código |
| [KIN-US-017](#kin-us-017) | Registrar integración de ticketing | BC04 | `TicketingIntegration`, `CreateIntegrationCommand` | Simulada o parcial |
| [KIN-US-018](#kin-us-018) | Sincronizar ocupación | BC04 | `TicketingIntegration`, `ShowOccupancy`, `SyncLog`, `SyncOccupancyCommand` | Simulada o parcial |
| [KIN-US-019](#kin-us-019) | Consultar ocupación de función | BC04 | `ShowOccupancy`, `GetOccupancyByShowIdQuery`, `TicketingQueryService` | Implementación en código |
| [KIN-US-020](#kin-us-020) | Inicializar asientos de función | BC05 | `SeatAllocation`, `ShowSeat`, `InitializeSeatsCommand` | Implementación en código |
| [KIN-US-021](#kin-us-021) | Consultar mapa y asiento | BC05 | `SeatAllocation`, `ShowSeat`, `SeatAllocationQueryService` | Implementación en código |
| [KIN-US-022](#kin-us-022) | Cambiar estado de asiento | BC05 | `SeatAllocation`, `ShowSeat`, `AssignSeatCommand` | Implementación en código |
| [KIN-US-023](#kin-us-023) | Retirar asiento de servicio con motivo | BC05 | `SeatAllocation`, `ShowSeat`, `ManualIntervention`, `MarkSeatOutOfServiceCommand` | Implementación en código |
| [KIN-US-024](#kin-us-024) | Iniciar ejecución 4D | BC06 | `ShowExecution`, `StartExecutionCommand` | Simulada o parcial |
| [KIN-US-025](#kin-us-025) | Pausar ejecución | BC06 | `ShowExecution`, `PauseExecutionCommand` | Implementación en código |
| [KIN-US-026](#kin-us-026) | Reanudar ejecución | BC06 | `ShowExecution`, `ResumeExecutionCommand` | Implementación en código |
| [KIN-US-027](#kin-us-027) | Finalizar ejecución | BC06 | `ShowExecution`, `FinishExecutionCommand` | Implementación en código |
| [KIN-US-028](#kin-us-028) | Registrar parada de emergencia | BC06 | `ShowExecution`, `EmergencyEvent`, `TriggerEmergencyStopCommand` | Simulada o parcial |
| [KIN-US-029](#kin-us-029) | Consultar ejecución | BC06 | `ShowExecution`, `SensorySequence`, `SynchronizationEvent`, `HardwareExecutionLog`, `EmergencyEvent`, `ShowExecutionQueryService` | Implementación en código |
| [KIN-US-030](#kin-us-030) | Registrar equipo | BC07 | `Equipment`, `RegisterEquipmentCommand` | Implementación en código |
| [KIN-US-031](#kin-us-031) | Reportar incidente | BC07 | `Equipment`, `Incident`, `RegisterIncidentCommand` | Implementación en código |
| [KIN-US-032](#kin-us-032) | Ejecutar prueba de equipo | BC07 | `Equipment`, `EquipmentTest`, `ChannelTest`, `RunEquipmentTestCommand` | Simulada o parcial |
| [KIN-US-033](#kin-us-033) | Programar mantenimiento | BC07 | `MaintenanceOrder`, `ScheduleMaintenanceCommand` | Implementación en código |
| [KIN-US-034](#kin-us-034) | Completar mantenimiento | BC07 | `MaintenanceOrder`, `Incident`, `CompleteMaintenanceCommand` | Implementación en código |
| [KIN-US-035](#kin-us-035) | Consultar equipos e historial de mantenimiento | BC07 | `Equipment`, `Incident`, `EquipmentTest`, `MaintenanceOrder`, `EquipmentQueryService`, `MaintenanceOrderQueryService` | Implementación en código |
| [KIN-US-036](#kin-us-036) | Generar dashboard | BC08 | `Dashboard`, `OperationalMetric`, `RoomPerformance`, `IncidentSummary`, `GenerateDashboardCommand` | Simulada o parcial |
| [KIN-US-037](#kin-us-037) | Consultar dashboard | BC08 | `Dashboard`, `DashboardQueryService`, `GetLatestDashboardQuery` | Implementación en código |
| [KIN-US-038](#kin-us-038) | Registrar reporte operativo | BC08 | `OperationalReport`, `GenerateReportCommand`, `OperationalReportCommandService` | Simulada o parcial |
| [KIN-US-039](#kin-us-039) | Consultar reporte registrado | BC08 | `OperationalReport`, `GetReportByIdQuery`, `OperationalReportQueryService` | Implementación en código |
| [KIN-US-040](#kin-us-040) | Consultar planes | BC09 | `SubscriptionPlan`, `GetAllPlansQuery`, `SubscriptionPlanQueryService` | Implementación en código |
| [KIN-US-041](#kin-us-041) | Crear suscripción | BC09 | `Subscription`, `SubscriptionPlan`, `CreateSubscriptionCommand` | Implementación en código |
| [KIN-US-042](#kin-us-042) | Procesar pago | BC09 | `Subscription`, `PaymentTransaction`, `ProcessPaymentCommand` | Simulada o parcial |
| [KIN-US-043](#kin-us-043) | Cambiar plan | BC09 | `Subscription`, `SubscriptionPlan`, `SubscriptionUpgrade`, `ChangePlanCommand` | Implementación en código |
| [KIN-US-044](#kin-us-044) | Consultar suscripción | BC09 | `Subscription`, `PaymentTransaction`, `SubscriptionUpgrade`, `SubscriptionQueryService` | Implementación en código |

## BC01 — Movies — Películas y contenido sensorial

<a id="kin-us-001"></a>

### KIN-US-001: Registrar película

**Descripción:** Como administrador de catálogo, quiero registrar una película con título, duración y género, para incorporarla al catálogo 4D.

**Estado:** Implementación en código.

**Elementos relacionados:** `Movie`, `CreateMovieCommand`, `MovieCommandService`.

#### Criterios de aceptación

**Escenario 1**

Dado un título no registrado y datos admitidos,  
cuando registro la película,  
entonces se crea Movie con identificador, estado Active y SensoryContent inicial.

**Escenario 2**

Dado una película con el mismo título,  
cuando intento registrarla,  
entonces se rechaza la operación por título duplicado.

**Evidencia:** [Movie.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Aggregates/Movie.cs); [CreateMovieCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Commands/CreateMovieCommand.cs); [MovieCommandService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Application/Internal/CommandServices/MovieCommandService.cs).

<a id="kin-us-002"></a>

### KIN-US-002: Actualizar película

**Descripción:** Como administrador de catálogo, quiero editar los datos de una película, para mantener actualizado el catálogo.

**Estado:** Implementación en código.

**Elementos relacionados:** `Movie`, `UpdateMovieCommand`, `MovieCommandService`.

#### Criterios de aceptación

**Escenario 1**

Dado una película existente y un título que no pertenece a otra película,  
cuando actualizo título, duración y género,  
entonces se conservan el identificador y los nuevos datos.

**Escenario 2**

Dado un identificador inexistente,  
cuando solicito la actualización,  
entonces se devuelve MovieNotFound.

**Límite observado:** Cambiar únicamente mayúsculas/minúsculas del título no se considera duplicado de la misma película.

**Evidencia:** [Movie.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Aggregates/Movie.cs); [UpdateMovieCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Commands/UpdateMovieCommand.cs); [MovieCommandService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Application/Internal/CommandServices/MovieCommandService.cs).

<a id="kin-us-003"></a>

### KIN-US-003: Consultar y buscar películas

**Descripción:** Como operador de cine, quiero listar películas, buscar por título o género y consultar su detalle, para encontrar el contenido que necesito.

**Estado:** Implementación en código.

**Elementos relacionados:** `Movie`, `MovieQueryService`, `GetAllMoviesQuery`, `SearchMoviesQuery`, `GetMovieByIdQuery`.

#### Criterios de aceptación

**Escenario 1**

Dado películas registradas,  
cuando solicito el catálogo o una búsqueda,  
entonces se devuelve la colección obtenida por la consulta correspondiente.

**Escenario 2**

Dado un identificador inexistente,  
cuando consulto su detalle,  
entonces la consulta devuelve ausencia de resultado.

**Evidencia:** [Movie.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Aggregates/Movie.cs); [MovieQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Application/Internal/QueryServices/MovieQueryService.cs); [GetAllMoviesQuery.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Queries/GetAllMoviesQuery.cs); [SearchMoviesQuery.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Queries/SearchMoviesQuery.cs); [GetMovieByIdQuery.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Queries/GetMovieByIdQuery.cs).

<a id="kin-us-004"></a>

### KIN-US-004: Asociar pista sensorial

**Descripción:** Como administrador de catálogo, quiero asociar un archivo de efectos a una película, para configurar sus canales sensoriales.

**Estado:** Implementación en código.

**Elementos relacionados:** `Movie`, `SensoryContent`, `SensoryTrack`, `AssociateSensoryFileCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una película existente sin pista para el tipo de efecto indicado,  
cuando asocio ruta y duración de una pista,  
entonces se agrega SensoryTrack a su contenido.

**Escenario 2**

Dado una pista del mismo tipo ya asociada,  
cuando intento agregar otra,  
entonces se rechaza con DuplicateEffectType.

**Límite observado:** La asociación guarda metadatos; no certifica carga, análisis ni reproducción de un archivo físico.

**Evidencia:** [Movie.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Aggregates/Movie.cs); [SensoryContent.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Entities/SensoryContent.cs); [SensoryTrack.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Entities/SensoryTrack.cs); [AssociateSensoryFileCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Commands/AssociateSensoryFileCommand.cs).

<a id="kin-us-005"></a>

### KIN-US-005: Configurar intensidad de efectos

**Descripción:** Como administrador de catálogo, quiero cambiar la intensidad sensorial, para adaptar el perfil de la película.

**Estado:** Implementación en código.

**Elementos relacionados:** `Movie`, `SensoryContent`, `ConfigurationHistoryEntry`, `ConfigureEffectsProfileCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado contenido con intensidad distinta a la solicitada,  
cuando cambio el nivel,  
entonces se actualiza y se registra intensidad anterior, nueva y fecha.

**Escenario 2**

Dado contenido con la misma intensidad solicitada,  
cuando repito la configuración,  
entonces no se añade otra entrada de historial.

**Evidencia:** [Movie.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Aggregates/Movie.cs); [SensoryContent.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Entities/SensoryContent.cs); [ConfigurationHistoryEntry.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Entities/ConfigurationHistoryEntry.cs); [ConfigureEffectsProfileCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Commands/ConfigureEffectsProfileCommand.cs).

<a id="kin-us-006"></a>

### KIN-US-006: Consultar configuración sensorial

**Descripción:** Como operador de cine, quiero consultar las pistas y configuración de una película, para revisar sus efectos disponibles.

**Estado:** Implementación en código.

**Elementos relacionados:** `SensoryContent`, `SensoryTrack`, `ConfigurationHistoryEntry`, `GetSensoryContentByMovieIdQuery`, `MovieQueryService`.

#### Criterios de aceptación

**Escenario 1**

Dado una película registrada,  
cuando consulto su contenido sensorial,  
entonces se obtiene SensoryContent cargado con sus detalles.

**Escenario 2**

Dado una película inexistente,  
cuando consulto su contenido sensorial,  
entonces la consulta devuelve ausencia de resultado.

**Evidencia:** [SensoryContent.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Entities/SensoryContent.cs); [SensoryTrack.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Entities/SensoryTrack.cs); [ConfigurationHistoryEntry.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Entities/ConfigurationHistoryEntry.cs); [GetSensoryContentByMovieIdQuery.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Queries/GetSensoryContentByMovieIdQuery.cs); [MovieQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Application/Internal/QueryServices/MovieQueryService.cs).

<a id="kin-us-007"></a>

### KIN-US-007: Desactivar película

**Descripción:** Como administrador de catálogo, quiero desactivar una película, para identificar contenido retirado del catálogo activo.

**Estado:** Implementación en código.

**Elementos relacionados:** `Movie`, `DeactivateMovieCommand`, `MovieCommandService`.

#### Criterios de aceptación

**Escenario 1**

Dado una película existente,  
cuando solicito desactivarla,  
entonces Movie.Status cambia a Inactive y se publica MovieDeactivatedEvent.

**Escenario 2**

Dado un identificador inexistente,  
cuando solicito desactivarlo,  
entonces se devuelve MovieNotFound.

**Límite observado:** No impide actualmente que Scheduling reciba su MovieId ni cancela funciones existentes.

**Evidencia:** [Movie.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Aggregates/Movie.cs); [DeactivateMovieCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Domain/Model/Commands/DeactivateMovieCommand.cs); [MovieCommandService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Movies/Application/Internal/CommandServices/MovieCommandService.cs).

## BC02 — Scheduling — Programación y calendario

<a id="kin-us-008"></a>

### KIN-US-008: Programar función

**Descripción:** Como programador de funciones, quiero asignar una película, sala e intervalo a una función, para organizar la programación.

**Estado:** Implementación en código.

**Elementos relacionados:** `Show`, `TimeSlot`, `ScheduleShowCommand`, `ShowCommandService`.

#### Criterios de aceptación

**Escenario 1**

Dado un intervalo con fin posterior al inicio y sin solapamiento en la sala,  
cuando programo la función,  
entonces se crea Show en estado Scheduled.

**Escenario 2**

Dado una función no cancelada que se solapa en la misma sala,  
cuando intento programar ese intervalo,  
entonces se rechaza por RoomAlreadyBooked.

**Escenario 3**

Dado un intervalo con fin igual o anterior al inicio,  
cuando intento programarlo,  
entonces se rechaza por InvalidTimeSlot.

**Límite observado:** No comprueba existencia/estado de Movie y Room, bloqueos de sala ni correspondencia con la duración de película.

**Evidencia:** [Show.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/Aggregates/Show.cs); [TimeSlot.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/ValueObjects/TimeSlot.cs); [ScheduleShowCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/Commands/ScheduleShowCommand.cs); [ShowCommandService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Application/Internal/CommandServices/ShowCommandService.cs).

<a id="kin-us-009"></a>

### KIN-US-009: Reprogramar función

**Descripción:** Como programador de funciones, quiero editar el intervalo de una función, para ajustar el calendario.

**Estado:** Implementación en código.

**Elementos relacionados:** `Show`, `TimeSlot`, `EditShowCommand`, `ShowCommandService`.

#### Criterios de aceptación

**Escenario 1**

Dado una función existente y nuevo intervalo válido sin conflicto,  
cuando edito su horario,  
entonces se reemplaza TimeSlot excluyendo la propia función de la búsqueda de conflictos.

**Escenario 2**

Dado un intervalo que se solapa con otra función no cancelada de la sala,  
cuando intento guardar,  
entonces se rechaza por TimeSlotOverlap.

**Evidencia:** [Show.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/Aggregates/Show.cs); [TimeSlot.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/ValueObjects/TimeSlot.cs); [EditShowCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/Commands/EditShowCommand.cs); [ShowCommandService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Application/Internal/CommandServices/ShowCommandService.cs).

<a id="kin-us-010"></a>

### KIN-US-010: Cancelar función

**Descripción:** Como programador de funciones, quiero cancelar una función, para retirarla de la programación operativa.

**Estado:** Implementación en código.

**Elementos relacionados:** `Show`, `CancelShowCommand`, `ShowCommandService`.

#### Criterios de aceptación

**Escenario 1**

Dado una función existente,  
cuando la cancelo,  
entonces su estado pasa a Cancelled y se publica ShowCancelledEvent.

**Escenario 2**

Dado una función cancelada,  
cuando se evalúa un nuevo horario en su sala,  
entonces esa función no bloquea el nuevo intervalo.

**Límite observado:** No se verificó propagación automática a Ticketing, Seats o Execution.

**Evidencia:** [Show.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/Aggregates/Show.cs); [CancelShowCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/Commands/CancelShowCommand.cs); [ShowCommandService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Application/Internal/CommandServices/ShowCommandService.cs).

<a id="kin-us-011"></a>

### KIN-US-011: Consultar calendario

**Descripción:** Como programador de funciones, quiero consultar todas las funciones o las de una fecha, para revisar la programación.

**Estado:** Implementación en código.

**Elementos relacionados:** `Show`, `ShowQueryService`, `GetDailyScheduleQuery`.

#### Criterios de aceptación

**Escenario 1**

Dado funciones con inicio en la fecha consultada,  
cuando solicito la programación diaria,  
entonces se obtienen por fecha de inicio y ordenadas por inicio.

**Escenario 2**

Dado ninguna función en la fecha,  
cuando consulto el calendario diario,  
entonces se devuelve una colección vacía.

**Límite observado:** La fecha se compara con la fecha de StartDateTime almacenada; no se documenta conversión a una zona horaria de negocio.

**Evidencia:** [Show.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/Aggregates/Show.cs); [ShowQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Application/Internal/QueryServices/ShowQueryService.cs); [GetDailyScheduleQuery.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Scheduling/Domain/Model/Queries/GetDailyScheduleQuery.cs).

## BC03 — Rooms — Preparación de salas

<a id="kin-us-012"></a>

### KIN-US-012: Registrar sala

**Descripción:** Como administrador de cine, quiero registrar una sala con nombre y capacidad, para incorporarla a la operación.

**Estado:** Implementación en código.

**Elementos relacionados:** `Room`, `CreateRoomCommand`, `RoomCommandService`.

#### Criterios de aceptación

**Escenario 1**

Dado un nombre de sala no registrado y datos admitidos,  
cuando creo la sala,  
entonces se guarda con capacidad indicada y estado Available.

**Escenario 2**

Dado un nombre ya registrado,  
cuando intento crear la sala,  
entonces se rechaza por DuplicateName.

**Evidencia:** [Room.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Aggregates/Room.cs); [CreateRoomCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Commands/CreateRoomCommand.cs); [RoomCommandService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Application/Internal/CommandServices/RoomCommandService.cs).

<a id="kin-us-013"></a>

### KIN-US-013: Preparar sala

**Descripción:** Como operador de sala, quiero registrar la preparación de una sala, para dejar constancia de su disponibilidad.

**Estado:** Simulada o parcial.

**Elementos relacionados:** `Room`, `RoomInspection`, `PrepareRoomCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una sala Available o Preparing sin bloqueos activos,  
cuando la preparo con observaciones,  
entonces se registra inspección Passed y la sala queda Available.

**Escenario 2**

Dado una sala en mantenimiento, fuera de servicio o con bloqueo activo,  
cuando intento prepararla,  
entonces se rechaza por RoomNotAvailableForPreparation.

**Límite observado:** La inspección se aprueba de forma fija; no existe evaluación real del checklist en este flujo.

**Evidencia:** [Room.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Aggregates/Room.cs); [RoomInspection.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Entities/RoomInspection.cs); [PrepareRoomCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Commands/PrepareRoomCommand.cs).

<a id="kin-us-014"></a>

### KIN-US-014: Bloquear sala

**Descripción:** Como operador de sala, quiero bloquear una sala indicando motivo e intervalo, para registrar una indisponibilidad.

**Estado:** Implementación en código.

**Elementos relacionados:** `Room`, `RoomBlock`, `BlockRoomCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una sala existente y un intervalo con fin posterior al inicio,  
cuando la bloqueo,  
entonces se agrega un RoomBlock activo y la sala pasa a Maintenance.

**Escenario 2**

Dado fin igual o anterior al inicio,  
cuando solicito el bloqueo,  
entonces se rechaza por InvalidBlockPeriod.

**Límite observado:** El estado cambia inmediatamente aunque el intervalo sea futuro; Scheduling no aplica este bloqueo.

**Evidencia:** [Room.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Aggregates/Room.cs); [RoomBlock.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Entities/RoomBlock.cs); [BlockRoomCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Commands/BlockRoomCommand.cs).

<a id="kin-us-015"></a>

### KIN-US-015: Liberar sala

**Descripción:** Como operador de sala, quiero liberar una sala bloqueada, para volver a disponer de ella.

**Estado:** Implementación en código.

**Elementos relacionados:** `Room`, `RoomBlock`, `ReleaseRoomCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una sala con bloqueos activos,  
cuando la libero,  
entonces todos los bloqueos activos se desactivan y queda Available.

**Escenario 2**

Dado una sala inexistente,  
cuando solicito liberarla,  
entonces se devuelve RoomNotFound.

**Evidencia:** [Room.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Aggregates/Room.cs); [RoomBlock.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Entities/RoomBlock.cs); [ReleaseRoomCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Commands/ReleaseRoomCommand.cs).

<a id="kin-us-016"></a>

### KIN-US-016: Consultar salas

**Descripción:** Como operador de cine, quiero listar salas y consultar inspecciones y bloqueos de una sala, para revisar su situación operativa.

**Estado:** Implementación en código.

**Elementos relacionados:** `Room`, `RoomInspection`, `RoomBlock`, `RoomQueryService`.

#### Criterios de aceptación

**Escenario 1**

Dado una sala registrada con inspecciones o bloqueos,  
cuando consulto su detalle,  
entonces se cargan sus inspecciones y bloqueos.

**Escenario 2**

Dado ninguna sala registrada,  
cuando solicito el listado,  
entonces se devuelve una colección vacía.

**Evidencia:** [Room.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Aggregates/Room.cs); [RoomInspection.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Entities/RoomInspection.cs); [RoomBlock.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Domain/Model/Entities/RoomBlock.cs); [RoomQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Rooms/Application/Internal/QueryServices/RoomQueryService.cs).

## BC04 — Ticketing — Integración de ticketing

<a id="kin-us-017"></a>

### KIN-US-017: Registrar integración de ticketing

**Descripción:** Como administrador de cine, quiero registrar el proveedor de ticketing de un cine, para preparar la consulta de ocupación.

**Estado:** Simulada o parcial.

**Elementos relacionados:** `TicketingIntegration`, `CreateIntegrationCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado CinemaId y proveedor admitidos,  
cuando registro la integración,  
entonces se guarda el proveedor con estado Connected y fecha de conexión.

**Escenario 2**

Dado una integración registrada,  
cuando consulto su identificador,  
entonces se obtiene con sus registros y ocupaciones.

**Límite observado:** Connected se asigna al registrar; no demuestra autenticación o conexión real al proveedor.

**Evidencia:** [TicketingIntegration.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Ticketing/Domain/Model/Aggregates/TicketingIntegration.cs); [CreateIntegrationCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Ticketing/Domain/Model/Commands/CreateIntegrationCommand.cs).

<a id="kin-us-018"></a>

### KIN-US-018: Sincronizar ocupación

**Descripción:** Como operador de cine, quiero actualizar la ocupación de una función desde ticketing, para conocer cuántos asientos están ocupados.

**Estado:** Simulada o parcial.

**Elementos relacionados:** `TicketingIntegration`, `ShowOccupancy`, `SyncLog`, `SyncOccupancyCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una integración existente y datos devueltos por el cliente externo,  
cuando sincronizo la función,  
entonces se crea o actualiza ShowOccupancy y se registra SyncLog con Success.

**Escenario 2**

Dado un fallo o ausencia de datos del proveedor,  
cuando solicito sincronizar,  
entonces se registra SyncLog Failed con detalle y no se actualiza la ocupación con datos nuevos.

**Escenario 3**

Dado una integración inexistente,  
cuando solicito sincronizar,  
entonces se devuelve IntegrationNotFound.

**Límite observado:** El cliente es simulado. No actualiza ShowSeat ni valida por sí solo los límites de ocupación.

**Evidencia:** [TicketingIntegration.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Ticketing/Domain/Model/Aggregates/TicketingIntegration.cs); [ShowOccupancy.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Ticketing/Domain/Model/Entities/ShowOccupancy.cs); [SyncLog.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Ticketing/Domain/Model/Entities/SyncLog.cs); [SyncOccupancyCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Ticketing/Domain/Model/Commands/SyncOccupancyCommand.cs).

<a id="kin-us-019"></a>

### KIN-US-019: Consultar ocupación de función

**Descripción:** Como operador de cine, quiero consultar la ocupación sincronizada de una función, para revisar la información de ticketing.

**Estado:** Implementación en código.

**Elementos relacionados:** `ShowOccupancy`, `GetOccupancyByShowIdQuery`, `TicketingQueryService`.

#### Criterios de aceptación

**Escenario 1**

Dado una ocupación registrada para ShowId,  
cuando la consulto,  
entonces se devuelve el registro obtenido por el repositorio.

**Escenario 2**

Dado ninguna ocupación registrada,  
cuando la consulto,  
entonces la consulta devuelve ausencia de resultado.

**Límite observado:** No se impone unicidad de ShowId entre integraciones; no se promete agregación de proveedores.

**Evidencia:** [ShowOccupancy.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Ticketing/Domain/Model/Entities/ShowOccupancy.cs); [GetOccupancyByShowIdQuery.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Ticketing/Domain/Model/Queries/GetOccupancyByShowIdQuery.cs); [TicketingQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Ticketing/Application/Internal/QueryServices/TicketingQueryService.cs).

## BC05 — Seats — Asignación y control de asientos

<a id="kin-us-020"></a>

### KIN-US-020: Inicializar asientos de función

**Descripción:** Como operador de cine, quiero crear el mapa de asientos de una función, para empezar a gestionar sus butacas.

**Estado:** Implementación en código.

**Elementos relacionados:** `SeatAllocation`, `ShowSeat`, `InitializeSeatsCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una función sin asignación y un TotalSeats positivo admitido,  
cuando inicializo sus asientos,  
entonces se crea una asignación con asientos numerados de 1 a TotalSeats, Available, Sold=false y Enabled=true.

**Escenario 2**

Dado una asignación ya existente para ShowId,  
cuando intento inicializar otra,  
entonces se rechaza por AllocationAlreadyExists.

**Límite observado:** TotalSeats procede del comando; no se contrasta con Room.Capacity ni se verifica existencia de Show.

**Evidencia:** [SeatAllocation.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Aggregates/SeatAllocation.cs); [ShowSeat.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Entities/ShowSeat.cs); [InitializeSeatsCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Commands/InitializeSeatsCommand.cs).

<a id="kin-us-021"></a>

### KIN-US-021: Consultar mapa y asiento

**Descripción:** Como operador de sala, quiero consultar el mapa y una butaca específica, para conocer su situación.

**Estado:** Implementación en código.

**Elementos relacionados:** `SeatAllocation`, `ShowSeat`, `SeatAllocationQueryService`.

#### Criterios de aceptación

**Escenario 1**

Dado una asignación existente,  
cuando consulto por ShowId,  
entonces se obtiene con asientos e intervenciones.

**Escenario 2**

Dado una asignación o número de asiento inexistente,  
cuando consulto ese asiento,  
entonces la consulta devuelve ausencia de resultado.

**Evidencia:** [SeatAllocation.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Aggregates/SeatAllocation.cs); [ShowSeat.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Entities/ShowSeat.cs); [SeatAllocationQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Application/Internal/QueryServices/SeatAllocationQueryService.cs).

<a id="kin-us-022"></a>

### KIN-US-022: Cambiar estado de asiento

**Descripción:** Como operador de sala, quiero modificar manualmente el estado de una butaca, para controlar su venta u operación.

**Estado:** Implementación en código.

**Elementos relacionados:** `SeatAllocation`, `ShowSeat`, `AssignSeatCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado un asiento existente y destino Sold, Enabled, Disabled u OutOfService,  
cuando asigno el estado,  
entonces se aplica el método correspondiente y se publica SeatStatusChangedEvent.

**Escenario 2**

Dado destino Available,  
cuando intento asignarlo manualmente,  
entonces se rechaza por UnsupportedSeatStatus.

**Escenario 3**

Dado un número de asiento inexistente en la asignación,  
cuando solicito el cambio,  
entonces se devuelve SeatNotFound.

**Límite observado:** Sold y Enabled son dimensiones separadas: habilitar no limpia Sold. AssignSeat hacia OutOfService no registra intervención.

**Evidencia:** [SeatAllocation.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Aggregates/SeatAllocation.cs); [ShowSeat.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Entities/ShowSeat.cs); [AssignSeatCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Commands/AssignSeatCommand.cs).

<a id="kin-us-023"></a>

### KIN-US-023: Retirar asiento de servicio con motivo

**Descripción:** Como operador de sala, quiero marcar una butaca fuera de servicio y registrar el motivo, para dejar trazabilidad de una intervención.

**Estado:** Implementación en código.

**Elementos relacionados:** `SeatAllocation`, `ShowSeat`, `ManualIntervention`, `MarkSeatOutOfServiceCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado un asiento existente,  
cuando uso la operación específica indicando motivo,  
entonces queda OutOfService, Enabled=false y se agrega ManualIntervention.

**Escenario 2**

Dado un asiento inexistente,  
cuando intento retirarlo,  
entonces se devuelve SeatNotFound sin registrar intervención.

**Límite observado:** El servicio actual utiliza OperatorId=1 por defecto; no identifica al operador autenticado.

**Evidencia:** [SeatAllocation.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Aggregates/SeatAllocation.cs); [ShowSeat.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Entities/ShowSeat.cs); [ManualIntervention.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Entities/ManualIntervention.cs); [MarkSeatOutOfServiceCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Seats/Domain/Model/Commands/MarkSeatOutOfServiceCommand.cs).

## BC06 — Execution — Ejecución y sincronización 4D

<a id="kin-us-024"></a>

### KIN-US-024: Iniciar ejecución 4D

**Descripción:** Como operador de proyección, quiero iniciar la ejecución de una función, para registrar el comienzo de su sesión 4D.

**Estado:** Simulada o parcial.

**Elementos relacionados:** `ShowExecution`, `StartExecutionCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado un ShowId sin ejecución InProgress o Paused,  
cuando inicio una ejecución con SensoryContentId,  
entonces se crea InProgress con StartedAt y se publica ExecutionStartedEvent.

**Escenario 2**

Dado una ejecución activa para ShowId,  
cuando intento iniciar otra,  
entonces se rechaza por ActiveExecutionExists.

**Límite observado:** No verifica Show/SensoryContent ni genera secuencias o envía órdenes físicas al hardware.

**Evidencia:** [ShowExecution.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Aggregates/ShowExecution.cs); [StartExecutionCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Commands/StartExecutionCommand.cs).

<a id="kin-us-025"></a>

### KIN-US-025: Pausar ejecución

**Descripción:** Como operador de proyección, quiero pausar una ejecución en curso, para registrar una interrupción temporal.

**Estado:** Implementación en código.

**Elementos relacionados:** `ShowExecution`, `PauseExecutionCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una ejecución InProgress,  
cuando solicito pausa,  
entonces queda Paused.

**Escenario 2**

Dado una ejecución en otro estado,  
cuando solicito pausa,  
entonces se rechaza por InvalidExecutionState.

**Límite observado:** El comportamiento documentado es el cambio de estado del backend; no se garantiza una pausa física.

**Evidencia:** [ShowExecution.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Aggregates/ShowExecution.cs); [PauseExecutionCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Commands/PauseExecutionCommand.cs).

<a id="kin-us-026"></a>

### KIN-US-026: Reanudar ejecución

**Descripción:** Como operador de proyección, quiero reanudar una ejecución pausada, para continuar la sesión.

**Estado:** Implementación en código.

**Elementos relacionados:** `ShowExecution`, `ResumeExecutionCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una ejecución Paused,  
cuando solicito reanudar,  
entonces queda InProgress.

**Escenario 2**

Dado una ejecución no pausada,  
cuando solicito reanudar,  
entonces se rechaza por InvalidExecutionState.

**Evidencia:** [ShowExecution.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Aggregates/ShowExecution.cs); [ResumeExecutionCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Commands/ResumeExecutionCommand.cs).

<a id="kin-us-027"></a>

### KIN-US-027: Finalizar ejecución

**Descripción:** Como operador de proyección, quiero finalizar una ejecución activa, para registrar el cierre de la sesión.

**Estado:** Implementación en código.

**Elementos relacionados:** `ShowExecution`, `FinishExecutionCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una ejecución InProgress o Paused,  
cuando la finalizo,  
entonces queda Finished, se asigna FinishedAt y se publica ExecutionFinishedEvent.

**Escenario 2**

Dado una ejecución Finished o EmergencyStopped,  
cuando intento finalizarla,  
entonces se rechaza por InvalidExecutionState.

**Límite observado:** No finaliza automáticamente Show ni acumula horas de Equipment.

**Evidencia:** [ShowExecution.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Aggregates/ShowExecution.cs); [FinishExecutionCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Commands/FinishExecutionCommand.cs).

<a id="kin-us-028"></a>

### KIN-US-028: Registrar parada de emergencia

**Descripción:** Como operador de proyección, quiero registrar una parada de emergencia con motivo, para dejar constancia de una situación crítica.

**Estado:** Simulada o parcial.

**Elementos relacionados:** `ShowExecution`, `EmergencyEvent`, `TriggerEmergencyStopCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una ejecución InProgress o Paused,  
cuando activo la parada con motivo,  
entonces queda EmergencyStopped y agrega EmergencyEvent Unresolved con fecha.

**Escenario 2**

Dado una ejecución terminada o ya detenida,  
cuando intento detenerla,  
entonces se rechaza por InvalidExecutionState.

**Límite observado:** No constituye una garantía de corte físico del hardware. No asigna FinishedAt.

**Evidencia:** [ShowExecution.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Aggregates/ShowExecution.cs); [EmergencyEvent.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Entities/EmergencyEvent.cs); [TriggerEmergencyStopCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Commands/TriggerEmergencyStopCommand.cs).

<a id="kin-us-029"></a>

### KIN-US-029: Consultar ejecución

**Descripción:** Como operador de proyección, quiero consultar una ejecución por ID o la activa de una función, para revisar estado y registros.

**Estado:** Implementación en código.

**Elementos relacionados:** `ShowExecution`, `SensorySequence`, `SynchronizationEvent`, `HardwareExecutionLog`, `EmergencyEvent`, `ShowExecutionQueryService`.

#### Criterios de aceptación

**Escenario 1**

Dado una ejecución registrada,  
cuando consulto su ID,  
entonces se obtiene con secuencias, sincronizaciones, logs y emergencias existentes.

**Escenario 2**

Dado una función sin ejecución InProgress o Paused,  
cuando consulto su ejecución activa,  
entonces la consulta devuelve ausencia de resultado.

**Límite observado:** SensorySequence.UpdateTimeCodes tiene lógica de desfase >500 ms; no se presenta como un flujo de telemetría integrado.

**Evidencia:** [ShowExecution.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Aggregates/ShowExecution.cs); [SensorySequence.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Entities/SensorySequence.cs); [SynchronizationEvent.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Entities/SynchronizationEvent.cs); [HardwareExecutionLog.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Entities/HardwareExecutionLog.cs); [EmergencyEvent.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Domain/Model/Entities/EmergencyEvent.cs); [ShowExecutionQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Execution/Application/Internal/QueryServices/ShowExecutionQueryService.cs).

## BC07 — Maintenance — Pruebas y mantenimiento

<a id="kin-us-030"></a>

### KIN-US-030: Registrar equipo

**Descripción:** Como técnico de mantenimiento, quiero registrar un equipo y su sala, para controlar los recursos 4D.

**Estado:** Implementación en código.

**Elementos relacionados:** `Equipment`, `RegisterEquipmentCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado nombre no utilizado en la sala y datos admitidos,  
cuando registro el equipo,  
entonces se crea Operational con fecha de instalación, vida útil y cero horas acumuladas.

**Escenario 2**

Dado mismo nombre en la misma sala,  
cuando intento registrarlo,  
entonces se rechaza por DuplicateEquipment.

**Límite observado:** No verifica la existencia de Room.

**Evidencia:** [Equipment.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Aggregates/Equipment.cs); [RegisterEquipmentCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Commands/RegisterEquipmentCommand.cs).

<a id="kin-us-031"></a>

### KIN-US-031: Reportar incidente

**Descripción:** Como técnico de mantenimiento, quiero registrar una incidencia con severidad, para documentar fallas de un equipo.

**Estado:** Implementación en código.

**Elementos relacionados:** `Equipment`, `Incident`, `RegisterIncidentCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado un equipo existente,  
cuando registro un incidente Critical,  
entonces se agrega el incidente y el equipo queda Faulty.

**Escenario 2**

Dado un equipo inexistente,  
cuando intento registrar un incidente,  
entonces se devuelve EquipmentNotFound.

**Evidencia:** [Equipment.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Aggregates/Equipment.cs); [Incident.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Entities/Incident.cs); [RegisterIncidentCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Commands/RegisterIncidentCommand.cs).

<a id="kin-us-032"></a>

### KIN-US-032: Ejecutar prueba de equipo

**Descripción:** Como técnico de mantenimiento, quiero probar un canal de un equipo, para obtener un diagnóstico registrado.

**Estado:** Simulada o parcial.

**Elementos relacionados:** `Equipment`, `EquipmentTest`, `ChannelTest`, `RunEquipmentTestCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado un equipo con AccumulatedHours menor que UsefulLifeHours,  
cuando ejecuto la prueba de un canal,  
entonces registra resultado Passed y deja Operational.

**Escenario 2**

Dado horas acumuladas iguales o superiores a la vida útil,  
cuando ejecuto la prueba,  
entonces registra Failed y deja Faulty.

**Límite observado:** Prueba simulada con intensidad 100; no consulta un sensor físico.

**Evidencia:** [Equipment.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Aggregates/Equipment.cs); [EquipmentTest.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Entities/EquipmentTest.cs); [ChannelTest.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Entities/ChannelTest.cs); [RunEquipmentTestCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Commands/RunEquipmentTestCommand.cs).

<a id="kin-us-033"></a>

### KIN-US-033: Programar mantenimiento

**Descripción:** Como técnico de mantenimiento, quiero programar una orden para un equipo, para organizar su atención.

**Estado:** Implementación en código.

**Elementos relacionados:** `MaintenanceOrder`, `ScheduleMaintenanceCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado un equipo existente y datos admitidos,  
cuando programo mantenimiento,  
entonces se guarda la orden Scheduled con tipo, fecha y el IncidentId opcional recibido.

**Escenario 2**

Dado un equipo inexistente,  
cuando intento programarlo,  
entonces se devuelve EquipmentNotFound.

**Límite observado:** No valida pertenencia del incidente al equipo ni bloquea la sala.

**Evidencia:** [MaintenanceOrder.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Aggregates/MaintenanceOrder.cs); [ScheduleMaintenanceCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Commands/ScheduleMaintenanceCommand.cs).

<a id="kin-us-034"></a>

### KIN-US-034: Completar mantenimiento

**Descripción:** Como técnico de mantenimiento, quiero registrar la conclusión de una orden, para dejar constancia del trabajo realizado.

**Estado:** Implementación en código.

**Elementos relacionados:** `MaintenanceOrder`, `Incident`, `CompleteMaintenanceCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado una orden existente,  
cuando la completo con notas,  
entonces queda Completed con fecha y notas.

**Escenario 2**

Dado una orden vinculada a un incidente que existe dentro del equipo,  
cuando la completo,  
entonces también se marca el incidente Resolved.

**Escenario 3**

Dado una orden inexistente,  
cuando solicito completarla,  
entonces se devuelve OrderNotFound.

**Límite observado:** No impone guardia de transición de la orden ni restaura Equipment.Status.

**Evidencia:** [MaintenanceOrder.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Aggregates/MaintenanceOrder.cs); [Incident.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Entities/Incident.cs); [CompleteMaintenanceCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Commands/CompleteMaintenanceCommand.cs).

<a id="kin-us-035"></a>

### KIN-US-035: Consultar equipos e historial de mantenimiento

**Descripción:** Como técnico de mantenimiento, quiero consultar equipos, incidentes y órdenes de un equipo, para revisar su historial.

**Estado:** Implementación en código.

**Elementos relacionados:** `Equipment`, `Incident`, `EquipmentTest`, `MaintenanceOrder`, `EquipmentQueryService`, `MaintenanceOrderQueryService`.

#### Criterios de aceptación

**Escenario 1**

Dado un equipo registrado,  
cuando consulto su detalle,  
entonces se cargan sus incidentes y pruebas.

**Escenario 2**

Dado un identificador de equipo,  
cuando solicito sus órdenes,  
entonces se devuelve la colección filtrada por EquipmentId.

**Escenario 3**

Dado un equipo inexistente,  
cuando consulto sus incidentes,  
entonces el servicio devuelve una colección vacía.

**Evidencia:** [Equipment.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Aggregates/Equipment.cs); [Incident.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Entities/Incident.cs); [EquipmentTest.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Entities/EquipmentTest.cs); [MaintenanceOrder.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Domain/Model/Aggregates/MaintenanceOrder.cs); [EquipmentQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Application/Internal/QueryServices/EquipmentQueryService.cs); [MaintenanceOrderQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Maintenance/Application/Internal/QueryServices/MaintenanceOrderQueryService.cs).

## BC08 — Analytics — Analítica y reportes

<a id="kin-us-036"></a>

### KIN-US-036: Generar dashboard

**Descripción:** Como administrador de cine, quiero generar un dashboard para un período, para revisar indicadores operativos.

**Estado:** Simulada o parcial.

**Elementos relacionados:** `Dashboard`, `OperationalMetric`, `RoomPerformance`, `IncidentSummary`, `GenerateDashboardCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado PeriodStart anterior a PeriodEnd,  
cuando genero el dashboard,  
entonces se persiste con fecha, métricas y resúmenes de ejemplo.

**Escenario 2**

Dado inicio igual o posterior al fin,  
cuando intento generarlo,  
entonces se rechaza por InvalidPeriod.

**Límite observado:** Las cifras son constantes; no se calculan desde la operación real.

**Evidencia:** [Dashboard.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Aggregates/Dashboard.cs); [OperationalMetric.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Entities/OperationalMetric.cs); [RoomPerformance.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Entities/RoomPerformance.cs); [IncidentSummary.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Entities/IncidentSummary.cs); [GenerateDashboardCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Commands/GenerateDashboardCommand.cs).

<a id="kin-us-037"></a>

### KIN-US-037: Consultar dashboard

**Descripción:** Como administrador de cine, quiero consultar un dashboard por ID o el más reciente, para revisar indicadores guardados.

**Estado:** Implementación en código.

**Elementos relacionados:** `Dashboard`, `DashboardQueryService`, `GetLatestDashboardQuery`.

#### Criterios de aceptación

**Escenario 1**

Dado un dashboard registrado,  
cuando consulto su identificador,  
entonces se obtiene con sus métricas y resúmenes.

**Escenario 2**

Dado ningún dashboard registrado,  
cuando solicito el más reciente,  
entonces la consulta devuelve ausencia de resultado.

**Evidencia:** [Dashboard.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Aggregates/Dashboard.cs); [DashboardQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Application/Internal/QueryServices/DashboardQueryService.cs); [GetLatestDashboardQuery.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Queries/GetLatestDashboardQuery.cs).

<a id="kin-us-038"></a>

### KIN-US-038: Registrar reporte operativo

**Descripción:** Como administrador de cine, quiero generar el registro de un reporte con tipo, período y formato, para organizar la información que deseo reportar.

**Estado:** Simulada o parcial.

**Elementos relacionados:** `OperationalReport`, `GenerateReportCommand`, `OperationalReportCommandService`.

#### Criterios de aceptación

**Escenario 1**

Dado un intervalo válido y tipo/formato admitidos,  
cuando genero el reporte,  
entonces se guardan metadatos y fecha de generación.

**Escenario 2**

Dado un intervalo inválido,  
cuando solicito el reporte,  
entonces se rechaza por InvalidPeriod.

**Límite observado:** No genera un archivo PDF/CSV ni consolida datos reales.

**Evidencia:** [OperationalReport.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Aggregates/OperationalReport.cs); [GenerateReportCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Commands/GenerateReportCommand.cs); [OperationalReportCommandService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Application/Internal/CommandServices/OperationalReportCommandService.cs).

<a id="kin-us-039"></a>

### KIN-US-039: Consultar reporte registrado

**Descripción:** Como administrador de cine, quiero consultar los metadatos de un reporte, para revisar su período y formato.

**Estado:** Implementación en código.

**Elementos relacionados:** `OperationalReport`, `GetReportByIdQuery`, `OperationalReportQueryService`.

#### Criterios de aceptación

**Escenario 1**

Dado un reporte existente,  
cuando consulto su ID,  
entonces se devuelve el registro.

**Escenario 2**

Dado un ID inexistente,  
cuando lo consulto,  
entonces la consulta devuelve ausencia de resultado.

**Evidencia:** [OperationalReport.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Aggregates/OperationalReport.cs); [GetReportByIdQuery.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Domain/Model/Queries/GetReportByIdQuery.cs); [OperationalReportQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Analytics/Application/Internal/QueryServices/OperationalReportQueryService.cs).

## BC09 — Subscriptions — Suscripciones

<a id="kin-us-040"></a>

### KIN-US-040: Consultar planes

**Descripción:** Como administrador de cine, quiero consultar los planes de suscripción, para comparar las opciones del servicio.

**Estado:** Implementación en código.

**Elementos relacionados:** `SubscriptionPlan`, `GetAllPlansQuery`, `SubscriptionPlanQueryService`.

#### Criterios de aceptación

**Escenario 1**

Dado planes registrados,  
cuando solicito el listado,  
entonces se devuelve la colección de planes con tipo, nombre, precio y MaxScreens.

**Escenario 2**

Dado ningún plan registrado,  
cuando solicito el listado,  
entonces se devuelve una colección vacía.

**Límite observado:** No implica que MaxScreens se aplique al registrar salas.

**Evidencia:** [SubscriptionPlan.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Aggregates/SubscriptionPlan.cs); [GetAllPlansQuery.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Queries/GetAllPlansQuery.cs); [SubscriptionPlanQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Application/Internal/QueryServices/SubscriptionPlanQueryService.cs).

<a id="kin-us-041"></a>

### KIN-US-041: Crear suscripción

**Descripción:** Como administrador de cine, quiero contratar un plan para mi cine, para registrar el servicio contratado.

**Estado:** Implementación en código.

**Elementos relacionados:** `Subscription`, `SubscriptionPlan`, `CreateSubscriptionCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado un plan existente,  
cuando creo una suscripción con CinemaId y PlanId,  
entonces se guarda Active con inicio actual y fin inicial a un mes.

**Escenario 2**

Dado un plan inexistente,  
cuando intento crear la suscripción,  
entonces se rechaza por PlanNotFound.

**Límite observado:** No verifica Cinema ni garantiza una sola suscripción por cine.

**Evidencia:** [Subscription.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Aggregates/Subscription.cs); [SubscriptionPlan.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Aggregates/SubscriptionPlan.cs); [CreateSubscriptionCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Commands/CreateSubscriptionCommand.cs).

<a id="kin-us-042"></a>

### KIN-US-042: Procesar pago

**Descripción:** Como administrador de cine, quiero registrar el pago de mi suscripción, para renovar su período de servicio.

**Estado:** Simulada o parcial.

**Elementos relacionados:** `Subscription`, `PaymentTransaction`, `ProcessPaymentCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado suscripción existente, importe positivo y token no vacío distinto de fail,  
cuando proceso el pago,  
entonces registra Successful, deja Active y extiende EndDate un mes desde el fin previo.

**Escenario 2**

Dado token fail sin distinción de mayúsculas,  
cuando proceso el pago,  
entonces registra Failed y deja PastDue.

**Escenario 3**

Dado importe no positivo o token vacío,  
cuando proceso el pago,  
entonces se rechaza por InvalidPayment sin crear transacción.

**Límite observado:** No hay pasarela real, moneda explícita ni verificación contra el precio del plan.

**Evidencia:** [Subscription.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Aggregates/Subscription.cs); [PaymentTransaction.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Entities/PaymentTransaction.cs); [ProcessPaymentCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Commands/ProcessPaymentCommand.cs).

<a id="kin-us-043"></a>

### KIN-US-043: Cambiar plan

**Descripción:** Como administrador de cine, quiero cambiar el plan de mi suscripción, para ajustar el servicio contratado.

**Estado:** Implementación en código.

**Elementos relacionados:** `Subscription`, `SubscriptionPlan`, `SubscriptionUpgrade`, `ChangePlanCommand`.

#### Criterios de aceptación

**Escenario 1**

Dado suscripción y plan destino existentes y distintos del actual,  
cuando cambio de plan,  
entonces se registra plan anterior/nuevo y se actualiza PlanId.

**Escenario 2**

Dado el plan actual como destino,  
cuando intento cambiarlo,  
entonces se rechaza por InvalidPlanChange.

**Escenario 3**

Dado un plan destino inexistente,  
cuando intento cambiarlo,  
entonces se rechaza por PlanNotFound.

**Límite observado:** No calcula prorrateo ni valida compatibilidad con salas existentes.

**Evidencia:** [Subscription.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Aggregates/Subscription.cs); [SubscriptionPlan.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Aggregates/SubscriptionPlan.cs); [SubscriptionUpgrade.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Entities/SubscriptionUpgrade.cs); [ChangePlanCommand.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Commands/ChangePlanCommand.cs).

<a id="kin-us-044"></a>

### KIN-US-044: Consultar suscripción

**Descripción:** Como administrador de cine, quiero consultar mi suscripción por ID o cine, para revisar su contrato.

**Estado:** Implementación en código.

**Elementos relacionados:** `Subscription`, `PaymentTransaction`, `SubscriptionUpgrade`, `SubscriptionQueryService`.

#### Criterios de aceptación

**Escenario 1**

Dado una suscripción existente,  
cuando consulto su identificador,  
entonces se obtiene con transacciones y cambios de plan.

**Escenario 2**

Dado un CinemaId sin suscripción,  
cuando consulto por cine,  
entonces la consulta devuelve ausencia de resultado.

**Límite observado:** La consulta por cine no demuestra unicidad de suscripción.

**Evidencia:** [Subscription.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Aggregates/Subscription.cs); [PaymentTransaction.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Entities/PaymentTransaction.cs); [SubscriptionUpgrade.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Domain/Model/Entities/SubscriptionUpgrade.cs); [SubscriptionQueryService.cs](https://github.com/kinemo-cinema/kinemo-platform/blob/d4b4c617061bb6729c7c3f8dbc42baad04f84df5/KinemoPlatform.API/Subscriptions/Application/Internal/QueryServices/SubscriptionQueryService.cs).

## Mejoras propuestas para validar con los requisitos de Kinemo

Estas propuestas **no forman parte de los criterios implementados anteriores**. Requieren definición de negocio y desarrollo antes de declararse completadas.

| Propuesta | BC | Resultado a definir o implementar |
|---|---|---|
| KIN-P-001 | Transversal / IAM | Autenticación real y autorización por rol; identidad del operador en intervenciones. IAM no es uno de los nueve contextos implementados actualmente. |
| KIN-P-002 | BC01 / BC02 / BC03 | Verificar existencia y estado de película/sala, contenido sensorial, duración de función y bloqueos antes de programar. |
| KIN-P-003 | BC02 / BC06 | Proteger frente a solicitudes concurrentes los solapamientos de sala y la creación de ejecuciones activas. |
| KIN-P-004 | BC03 | Evaluación real de inspecciones; reglas temporales para activación/vencimiento de bloqueos. |
| KIN-P-005 | BC04 / BC05 | Proveedor de ticketing real, límites y unicidad de ocupación, correspondencia con asientos y sincronización entre contextos. |
| KIN-P-006 | BC05 | Definir coherencia entre Sold, Enabled y Status, validar capacidad real y registrar toda salida de servicio por ambos caminos. |
| KIN-P-007 | BC06 | Crear secuencias desde las pistas y conectar telemetría, sincronización y parada física de emergencia. |
| KIN-P-008 | BC07 | Diagnóstico físico, acumulación de horas, pertenencia del incidente y transiciones válidas de mantenimiento. |
| KIN-P-009 | BC08 | Calcular indicadores con datos operativos y producir archivos PDF/CSV reales. |
| KIN-P-010 | BC09 | Integrar pagos reales, definir moneda, precio/prorrateo, unicidad por cine y aplicación de MaxScreens. |

## Correspondencia con el diagrama

Las historias hacen referencia a las 12 raíces de agregado del PlantUML: Movie, Show, Room, TicketingIntegration, SeatAllocation, ShowExecution, Equipment, MaintenanceOrder, Dashboard, OperationalReport, Subscription y SubscriptionPlan.

Los comandos y servicios se incluyen en la RTM para seguir la implementación aunque se hayan omitido del diagrama centrado en dominio. TimeSlot se usa en la programación; las entidades dependientes se vinculan a las historias de su agregado.

ScheduleConflict existe como entidad persistida, pero el flujo de programación revisado rechaza solapamientos sin crear registros de conflicto. Los métodos de SensorySequence, EmergencyEvent y HardwareExecutionLog no demuestran por sí mismos un flujo completo de monitoreo; no se inventaron historias “terminadas” de telemetría física. Los eventos revisados registran mensajes, sin implementar automáticamente los efectos entre contextos propuestos.

## Validación documental

Se verificó que hay 44 identificadores únicos, cobertura de los nueve bounded contexts y al menos dos escenarios por historia. Se contrastaron los flujos con los servicios de comandos, de consultas y las clases de dominio revisadas. Esta es una revisión documental y de código; no se ejecutaron estas historias como pruebas de aceptación.

