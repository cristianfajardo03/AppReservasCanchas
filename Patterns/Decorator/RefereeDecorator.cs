using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.Decorator;

public sealed class RefereeDecorator : ReservationDecorator
{
    public const decimal Surcharge = 40_000m;

    public RefereeDecorator(IReservationComponent inner) : base(inner)
    {
    }

    public override string GetDescription() => $"{Inner.GetDescription()} + Árbitro";

    public override decimal GetPrice() => Inner.GetPrice() + Surcharge;
}
