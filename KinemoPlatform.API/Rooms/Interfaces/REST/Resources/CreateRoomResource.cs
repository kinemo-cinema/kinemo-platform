using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

public record CreateRoomResource([Required(ErrorMessage = "RequiredRoomName")]
    [StringLength(100, ErrorMessage = "InvalidRoomNameLength")]
    string Name,
    [Range(1, 1000, ErrorMessage = "InvalidRoomCapacity")]
    int Capacity);