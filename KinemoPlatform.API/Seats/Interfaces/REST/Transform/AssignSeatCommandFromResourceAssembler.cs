using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Seats.Interfaces.REST.Resources;
using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Transform;

public static class AssignSeatCommandFromResourceAssembler
{
    public static AssignSeatCommand ToCommandFromResource(AssignSeatResource resource, int showId, int seatNumber)
    {
        return new AssignSeatCommand(showId, seatNumber, resource.NewStatus, resource.Reason);
    }
}