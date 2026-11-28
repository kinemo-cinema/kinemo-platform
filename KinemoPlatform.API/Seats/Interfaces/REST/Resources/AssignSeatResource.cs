using System.ComponentModel.DataAnnotations;
using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Resources;

public record AssignSeatResource(
    [EnumDataType(typeof(ESeatStatus), ErrorMessage = "InvalidSeatStatus")]
    ESeatStatus NewStatus,
    [StringLength(500, ErrorMessage = "InvalidOutOfServiceReasonLength")]
    string? Reason = null);
