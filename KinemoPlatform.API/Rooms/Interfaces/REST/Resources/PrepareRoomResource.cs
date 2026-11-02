using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

public record PrepareRoomResource([Required(ErrorMessage = "RequiredObservations")]
    [StringLength(1000, ErrorMessage = "InvalidObservationsLength")]
    string Observations);