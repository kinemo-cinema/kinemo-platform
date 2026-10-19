using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.Commands;

namespace KinemoPlatform.API.Scheduling.Domain.Services;

/// <summary>
///     Represents the show command service in the Kinemo Platform.
/// </summary>
public interface IShowCommandService
{
    /// <summary>
    ///     Handles the scheduling of a new show (US07).
    /// </summary>
    /// <param name="command">The <see cref="ScheduleShowCommand" />.</param>
    /// <returns>The scheduled <see cref="Show" />, or null if the room is already occupied.</returns>
    Task<Show?> Handle(ScheduleShowCommand command);

    /// <summary>
    ///     Handles the editing of an existing show's time slot (US08).
    /// </summary>
    /// <param name="command">The <see cref="EditShowCommand" />.</param>
    /// <returns>The updated <see cref="Show" />, or null if not found or overlap occurs.</returns>
    Task<Show?> Handle(EditShowCommand command);

    /// <summary>
    ///     Handles the cancellation of an existing show (US08).
    /// </summary>
    /// <param name="command">The <see cref="CancelShowCommand" />.</param>
    /// <returns>The cancelled <see cref="Show" />, or null if not found.</returns>
    Task<Show?> Handle(CancelShowCommand command);
}