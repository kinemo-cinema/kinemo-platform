using KinemoPlatform.API.Scheduling.Application.Errors;
using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Scheduling.Application.Services;

/// <summary>
///     Represents the show command service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Every operation returns a success result with the affected <see cref="Show" />, or a failure with an
///     <see cref="EShowError" />.
/// </remarks>
public interface IShowCommandService
{
    /// <summary>
    ///     Handles the scheduling of a new show (US07).
    /// </summary>
    /// <param name="command">The <see cref="ScheduleShowCommand" />.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     The scheduled <see cref="Show" />, or <see cref="EShowError.InvalidTimeSlot" /> /
    ///     <see cref="EShowError.RoomAlreadyBooked" />.
    /// </returns>
    Task<Result<Show, EShowError>> Handle(ScheduleShowCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the editing of an existing show's time slot (US08).
    /// </summary>
    /// <param name="command">The <see cref="EditShowCommand" />.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     The updated <see cref="Show" />, or <see cref="EShowError.ShowNotFound" /> /
    ///     <see cref="EShowError.InvalidTimeSlot" /> / <see cref="EShowError.TimeSlotOverlap" />.
    /// </returns>
    Task<Result<Show, EShowError>> Handle(EditShowCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the cancellation of an existing show (US08).
    /// </summary>
    /// <param name="command">The <see cref="CancelShowCommand" />.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The cancelled <see cref="Show" />, or <see cref="EShowError.ShowNotFound" />.</returns>
    Task<Result<Show, EShowError>> Handle(CancelShowCommand command, CancellationToken cancellationToken = default);
}