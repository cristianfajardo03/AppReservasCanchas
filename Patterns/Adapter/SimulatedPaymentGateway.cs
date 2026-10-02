namespace AppReservasCanchas.Patterns.Adapter;

/// <summary>
/// API simulada con forma ajena a la de la aplicación. No usa red.
/// </summary>
public sealed class SimulatedPaymentGateway
{
    public string ExecuteCharge(PaymentGatewayRequest request)
    {
        if (request.AmountCents <= 0)
        {
            return "13";
        }

        return request.Approve ? "00" : "05";
    }
}
