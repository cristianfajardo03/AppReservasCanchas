namespace AppReservasCanchas.Patterns.Adapter;

public sealed class PaymentGatewayRequest
{
    public int AmountCents { get; init; }

    public string MerchantRef { get; init; } = string.Empty;

    public bool Approve { get; init; }
}
