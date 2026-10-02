namespace AppReservasCanchas.Patterns.Adapter;

public sealed class SimulatedPaymentAdapter : IPaymentProcessor
{
    private readonly SimulatedPaymentGateway _gateway;

    public SimulatedPaymentAdapter(SimulatedPaymentGateway gateway)
    {
        _gateway = gateway;
    }

    public PaymentResult Charge(decimal amount, string reference, bool approve)
    {
        string code = _gateway.ExecuteCharge(new PaymentGatewayRequest
        {
            AmountCents = (int)decimal.Round(amount, 0, MidpointRounding.AwayFromZero),
            MerchantRef = reference,
            Approve = approve
        });

        return new PaymentResult
        {
            Success = code == "00",
            Code = code
        };
    }
}
