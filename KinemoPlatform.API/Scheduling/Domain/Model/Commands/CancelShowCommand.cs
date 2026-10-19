namespace KinemoPlatform.API.Scheduling.Domain.Model.Commands;

/// <summary>
///     Command to cancel a scheduled show.
/// </summary>
/// <param name="ShowId">
///     The unique identifier of the show to cancel.
/// </param>
public record CancelShowCommand(int ShowId);