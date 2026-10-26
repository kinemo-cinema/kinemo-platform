using KinemoPlatform.API.Ticketing.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

public record SyncLogResource(int Id, int ShowId, DateTimeOffset ExecutedAt, ESyncStatus Status, string ErrorDetails);