using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.FactoryMethod;

public sealed class Futbol8Creator : CourtCreator
{
    public override IReservationComponent CreateCourt() => new Futbol8Court();
}
