using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Resources;

public record StartExecutionResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidExecutionShowId")] int ShowId,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidSensoryContentId")] int SensoryContentId);