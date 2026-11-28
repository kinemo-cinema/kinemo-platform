namespace KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

/// <summary>Only <c>active: false</c> is accepted: a block can be lifted, never reactivated.</summary>
public record UpdateRoomBlockResource(bool Active);