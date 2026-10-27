namespace KinemoPlatform.API.Scheduling.Domain.Model.Commands;

/// <summary>
///     Command to edit the schedule of an existing show.
/// </summary>
/// <param name="ShowId">
///     The unique identifier of the show to edit.
/// </param>
/// <param name="StartDateTime">
///     The new start date and time.
/// </param>
/// <param name="EndDateTime">
///     The new end date and time.
/// </param>
public record EditShowCommand(int ShowId, DateTimeOffset StartDateTime, DateTimeOffset EndDateTime);