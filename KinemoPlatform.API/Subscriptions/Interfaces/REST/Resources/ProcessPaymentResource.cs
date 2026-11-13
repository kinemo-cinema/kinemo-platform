using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;

public record ProcessPaymentResource(
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    decimal Amount,

    [Required]
    [MinLength(1)]
    string PaymentToken
);