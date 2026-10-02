using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.Decorator;

public abstract class ReservationDecorator : IReservationComponent
{
    protected ReservationDecorator(IReservationComponent inner)
    {
        Inner = inner;
    }

    protected IReservationComponent Inner { get; }

    public abstract string GetDescription();

    public abstract decimal GetPrice();
}
