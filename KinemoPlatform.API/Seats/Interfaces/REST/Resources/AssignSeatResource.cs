using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Resources;

public record AssignSeatResource(ESeatStatus NewStatus);