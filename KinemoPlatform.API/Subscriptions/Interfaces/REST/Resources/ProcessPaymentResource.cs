using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;

public record ProcessPaymentResource(
    [property: Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    decimal Amount,

    [property: Required]
    [property: MinLength(1)]
    string PaymentToken
);