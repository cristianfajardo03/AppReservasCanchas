namespace AppReservasCanchas.Patterns.Adapter;

public sealed class PaymentResult
{
    public bool Success { get; init; }

    public string Code { get; init; } = string.Empty;
}
