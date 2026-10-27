using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Seats.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Transform;

public static class InitializeSeatsCommandFromResourceAssembler
{
    public static InitializeSeatsCommand ToCommandFromResource(InitializeSeatsResource resource)
    {
        return new InitializeSeatsCommand(resource.ShowId, resource.TotalSeats);
    }
}