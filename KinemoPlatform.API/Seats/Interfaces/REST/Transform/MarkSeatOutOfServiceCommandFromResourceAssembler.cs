using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Seats.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Transform;

public static class MarkSeatOutOfServiceCommandFromResourceAssembler
{
    public static MarkSeatOutOfServiceCommand ToCommandFromResource(MarkSeatOutOfServiceResource resource, int showId, int seatNumber)
    {
        return new MarkSeatOutOfServiceCommand(showId, seatNumber, resource.Reason);
    }
}