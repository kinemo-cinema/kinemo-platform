using System.ComponentModel.DataAnnotations;
namespace KinemoPlatform.API.Seats.Interfaces.REST.Resources;

public record InitializeSeatsResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidSeatShowId")]
    int ShowId,
    [Range(1, 1000, ErrorMessage = "InvalidTotalSeats")]
    int TotalSeats);
