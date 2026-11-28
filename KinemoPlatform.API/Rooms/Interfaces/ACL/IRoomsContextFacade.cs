namespace KinemoPlatform.API.Rooms.Interfaces.ACL;

/// <summary>Facade that exposes Rooms data to other bounded contexts.</summary>
public interface IRoomsContextFacade
{
    /// <summary>Ids of all registered rooms.</summary>
    Task<IReadOnlyList<int>> FetchRoomIds(CancellationToken cancellationToken = default);
}