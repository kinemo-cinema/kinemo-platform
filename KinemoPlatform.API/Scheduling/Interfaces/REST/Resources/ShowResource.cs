using KinemoPlatform.API.Scheduling.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Resources;

public record ShowResource(
    int Id,
    int MovieId,
    int RoomId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime,
    EShowStatus Status);