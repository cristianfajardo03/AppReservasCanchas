namespace AppReservasCanchas.Patterns.Adapter;

public interface IPaymentProcessor
{
    PaymentResult Charge(decimal amount, string reference, bool approve);
}
