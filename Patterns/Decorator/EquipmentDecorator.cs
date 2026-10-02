using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.Decorator;

public sealed class EquipmentDecorator : ReservationDecorator
{
    public const decimal Surcharge = 25_000m;

    public EquipmentDecorator(IReservationComponent inner) : base(inner)
    {
    }

    public override string GetDescription() => $"{Inner.GetDescription()} + Equipamiento deportivo";

    public override decimal GetPrice() => Inner.GetPrice() + Surcharge;
}
