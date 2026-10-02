using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.Decorator;

public sealed class LightingDecorator : ReservationDecorator
{
    public const decimal Surcharge = 15_000m;

    public LightingDecorator(IReservationComponent inner) : base(inner)
    {
    }

    public override string GetDescription() => $"{Inner.GetDescription()} + Iluminación";

    public override decimal GetPrice() => Inner.GetPrice() + Surcharge;
}
